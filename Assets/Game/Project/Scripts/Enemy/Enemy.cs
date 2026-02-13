using Game.Project.Data.Damage;
using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Enemy.EnemySO;
using Game.Project.Scripts.Enemy.Interface;
using Game.Project.Scripts.Managers.Singleton;
using System;
using UnityEngine;
using System.Collections;

namespace Game.Project.Scripts.Enemy
{
    /// <summary>
    /// 모든 적 캐릭터의 베이스가 되는 추상 클래스입니다.
    /// IDamageable 인터페이스를 상속받아 대미지 판정을 처리합니다.
    /// </summary>
    public abstract class Enemy : MonoBehaviour, IDamageable
    {
        public event Action<EnemyStateType> OnStateChanged;
        public event Action OnEnemyDead;

        protected EnemyContext context;                 // 현재 몬스터의 런타임 데이터
        protected EnemyCombat enemyCombat;              // 적 공격 담당
        protected EnemyStateMachine stateMachine;       // 상태 머신

        public EnemyContext Context => context;
        public EnemyData Data => context.data;
        public EnemyCombat EnemyCombat => enemyCombat;
        private readonly Collider[] _neighborResults = new Collider[10];

        protected float currentHp;
        protected bool isDead;
        private bool _canRotate = true;                  // 공격 중 회전 잠금을 위한 플래그

        protected Animator animator;
        protected Rigidbody rb;
        private EnemyHPBar _hpBar;

        private int _enemyLayer; // 캐싱된 레이어 번호

        // 문자열 대신 해시값을 사용하여 애니메이터 성능을 최적화
        protected readonly int animMoveHash = Animator.StringToHash("isMove");
        protected readonly int animAttackHash = Animator.StringToHash("attack");
        protected readonly int animDeadHash = Animator.StringToHash("die");

        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            rb = GetComponentInChildren<Rigidbody>();
            enemyCombat = GetComponentInChildren<EnemyCombat>();
            _enemyLayer = LayerMask.NameToLayer("Enemy");
        }

        /// <summary>
        /// 풀에서 재사용 시 필요한 초기화 값
        /// </summary>
        public virtual void Init(EnemyContext ctx)
        {
            context = ctx;
            context.owner = gameObject;
            currentHp = context.currentStat.maxHp;
            isDead = false;

            gameObject.layer = _enemyLayer;
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.velocity = Vector3.zero;
            }

            if (_hpBar == null) _hpBar = GetComponentInChildren<EnemyHPBar>();
            _hpBar?.Init(this);
            enemyCombat?.Init(this);

            stateMachine = new EnemyStateMachine();
            SetupStates();

            ChangeState(EnemyStateType.Spawn);
        }

        protected abstract void SetupStates(); // 자식 클래스에서 상태를 등록

        protected virtual void Update()
        {
            stateMachine.Execute();
        }

        /// <summary>
        /// 몬스터의 상태를 변경 용
        /// </summary>
        public void ChangeState(EnemyStateType newState)
        {
            if (isDead && newState != EnemyStateType.Dead) return;

            stateMachine.ChangeState(newState);
            OnStateChanged?.Invoke(newState);
        }

        /// <summary>
        /// 대미지를 입을 때 호출 ( 인터페이스 구현 )
        /// </summary>
        public virtual void TakeDamage(ProjectileContext context)
        {
            if (isDead || context == null) return;

            float rawDamage = context.finalDamage;
            float defense = Mathf.Max(0, this.context.currentStat.defense);
            float damageReduction = 100f / (100f + defense);
            float finalDamage = rawDamage * damageReduction;

            currentHp -= finalDamage;

            enemyCombat?.ShowDamageEffect(finalDamage, context.isCritical);

            _hpBar?.UpdateHP(currentHp);

            if (currentHp <= 0)
            {
                ChangeState(EnemyStateType.Dead);
            }
        }

        public virtual void OnDead()
        {
            if (isDead) return;
            isDead = true;

            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            StopMoving();
            if (rb != null) rb.isKinematic = true;

            animator?.SetTrigger(animDeadHash);
            OnEnemyDead?.Invoke();
        }

        /// <summary>
        /// 물리 이동 
        /// </summary>
        public void Move(Vector3 target, float speed)
        {
            if (rb == null || isDead) return;

            Vector3 dir = (target - transform.position);
            dir.y = 0;

            if (dir.sqrMagnitude > 0.001f)
            {
                Vector3 normalizedDir = dir.normalized;
                rb.velocity = new Vector3(0, rb.velocity.y, 0);

                Vector3 nextPos = transform.position + (normalizedDir + GetSeparationVector()) * speed * Time.deltaTime;
                rb.MovePosition(nextPos);

                transform.forward = normalizedDir;
            }
        }

        public void LookAtTarget(Vector3 target)
        {
            if (!_canRotate || isDead) return;

            Vector3 dir = target - transform.position;
            dir.y = 0;

            if (dir.sqrMagnitude > 0.01f)
                transform.forward = dir.normalized;
        }

        public void StopMoving()
        {
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        /// <summary>
        /// 대상 거리 계산.
        /// </summary>
        public float GetTargetDistance()
        {
            if (context.target == null) return float.MaxValue;

            Vector3 myPos = transform.position;
            Vector3 targetPos = context.target.transform.position;
            myPos.y = 0; targetPos.y = 0;

            return Vector3.Distance(myPos, targetPos);
        }

        /// <summary>
        /// 몬스터들이 서로 겹치지 않게 밀어내는 벡터 연산
        /// </summary>
        public Vector3 GetSeparationVector()
        {
            Vector3 separation = Vector3.zero;
            int monsterLayer = 1 << _enemyLayer;
            float separationDistance = 1.5f;

            int hitCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                separationDistance,
                _neighborResults,
                monsterLayer
            );

            for (int i = 0; i < hitCount; i++)
            {
                Collider neighbor = _neighborResults[i];

                if (neighbor.gameObject == gameObject) continue;

                Vector3 diff = transform.position - neighbor.transform.position;
                float distance = diff.magnitude;

                if (distance < separationDistance && distance > 0.001f)
                {
                    separation += diff.normalized * (separationDistance - distance);
                }
            }
            return separation;
        }

        public void SetMoveAnim(float value) => animator?.SetFloat(animMoveHash, value);
        public void SetAttackAnim() => animator?.SetTrigger(animAttackHash);
        public void ActiveAttack() => enemyCombat?.Attack();
        public void SetRotationLock(bool isLocked) => _canRotate = !isLocked;
        public LayerMask GetPlayerLayerMask() => LayerMask.GetMask("Player");
    }
}