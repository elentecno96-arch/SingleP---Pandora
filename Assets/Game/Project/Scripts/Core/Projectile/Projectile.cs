using Game.Project.Data.Damage;
using Game.Project.Scripts.Core.Projectile.Interface;
using Game.Project.Scripts.Core.Projectile.States;
using Game.Project.Scripts.Managers.Singleton;
using System;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile
{
    /// <summary>
    /// 투사체 본채
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        private ProjectileContext _context;
        private IProjectileState _currentState;
        private IProjectileMover _mover;
        private bool _isReturned = false;
        private bool _hasImpacted = false;
        public float StateTimer { get; set; }

        [SerializeField] private LayerMask targetMask;
        public LayerMask TargetMask => targetMask;

        public event Action OnSpawn;
        public event Action OnCharge;
        public event Action OnFly;
        public event Action<GameObject> OnImpact;
        public event Action OnChargeExit;
        public event Action OnFlyExit;

        /// <summary>
        /// PoolManager에서 콜백하고 있음 (의도적)
        /// </summary>
        public Action<Projectile> OnReturnToPool;

        public ProjectileContext Context => _context;
        public IProjectileMover Mover => _mover;

        public void Init(ProjectileContext context, IProjectileMover mover)
        {
            _isReturned = false;
            _hasImpacted = false;
            StateTimer = 0;
            ClearAllEvents();

            _context = context;
            _mover = mover;

            this.targetMask = _context.targetMask | LayerMask.GetMask("Ground");

            transform.position = _context.firePosition;
            if (_context.direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(_context.direction);

            if (TryGetComponent(out ProjectileVisual visual)) visual.Bind();
            if (TryGetComponent(out ProjectileAudio audio)) audio.Bind();

            _mover.Init(_context, this);

            transform.localScale = Vector3.one * _context.finalScale;

            ChangeState(ProjectileStates.Spawn);
        }

        /// <summary>
        /// 투사체 마스크 설정
        /// </summary>
        /// <param name="newMask"></param>
        public void SetTargetMask(LayerMask newMask)
        {
            targetMask = newMask;
        }
        public void ChangeState(IProjectileState newState)
        {
            if (_currentState != null)
            {
                if (_currentState == ProjectileStates.Fly) OnFlyExit?.Invoke();
                if (_currentState == ProjectileStates.Charge) OnChargeExit?.Invoke();

                _currentState.Exit(_context);
            }

            _currentState = newState;

            if (_currentState != null)
            {
                if (_currentState == ProjectileStates.Spawn) OnSpawn?.Invoke();
                else if (_currentState == ProjectileStates.Charge) OnCharge?.Invoke();
                else if (_currentState == ProjectileStates.Fly) OnFly?.Invoke();

                _currentState.Enter(_context, this);
            }
        }
        private void Update() => _currentState?.UpdateState(this);
        private void OnTriggerEnter(Collider other)
        {
            if (_isReturned || _context == null || _hasImpacted) return;
            if (other.gameObject == _context.owner) return;

            int otherLayerMask = 1 << other.gameObject.layer;
            if ((targetMask.value & otherLayerMask) != 0)
            {
                bool isDamageable = other.TryGetComponent(out IDamageable targetInterface);

                if (isDamageable)
                {
                    bool shouldImpact = true;
                    if (_mover is IProjectileHitable hitHandler)
                    {
                        shouldImpact = hitHandler.OnHit(this, other);
                    }

                    if (shouldImpact)
                    {
                        ExecuteImpact(other.gameObject);
                    }
                    else
                    {
                        ImpactStateCall(other.gameObject);
                    }
                }
                else
                {
                    ExecuteImpact(other.gameObject);
                }
            }
        }

        private void ExecuteImpact(GameObject target)
        {
            _hasImpacted = true;
            _context.target = target;
            ChangeState(ProjectileStates.Impact);
        }

        public void ReturnToPool()
        {
            if (_isReturned) return;
            _isReturned = true;

            StopAllCoroutines();
            if (_mover is IDisposable disposable) disposable.Dispose();
            if (TryGetComponent(out ProjectileVisual visual)) visual.Unbind();

            _context = null;
            _mover = null;
            _currentState = null;

            ClearAllEvents();
            OnReturnToPool?.Invoke(this);
        }
        private void ClearAllEvents()
        {
            OnSpawn = null;
            OnCharge = null;
            OnFly = null;
            OnImpact = null;
            OnChargeExit = null;
            OnFlyExit = null;
        }
        public void ImpactStateCall(GameObject target)
        {
            Debug.Log($"<color=cyan>[ImpactCall]</color> 호출됨! 대상: {target.name}, 프레임: {Time.frameCount}");
            if (_isReturned || _context == null) return;

            OnImpact?.Invoke(target);

            if (target != null && target.TryGetComponent(out IDamageable dmg))
            {
                if (_context.isCritical)
                {
                    var dispatchContext = _context.Clone();
                    dispatchContext.finalDamage *= _context.finalCritDamage;
                    dmg.TakeDamage(dispatchContext);
                }
                else
                {
                    dmg.TakeDamage(_context);

                }
            }
        }
    }
}

