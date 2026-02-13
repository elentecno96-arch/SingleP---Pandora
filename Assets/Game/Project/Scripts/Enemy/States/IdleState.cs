using Game.Project.Scripts.Enemy.Interface;
using UnityEngine;

namespace Game.Project.Scripts.Enemy.States
{
    /// <summary>
    /// 적의 대기 상태
    /// </summary>
    public class IdleState : IEnemyState
    {
        private readonly Enemy _owner;
        private float _timer;

        private const float IDLE_DURATION = 0.2f;

        public IdleState(Enemy owner) => _owner = owner;

        public void Enter()
        {
            _timer = 0f;
            _owner.SetMoveAnim(0f);
        }

        public void Execute()
        {
            _timer += Time.deltaTime;

            if (_owner.Context.target == null)
            {
                SearchTarget();
                return;
            }
            TargetFind();
        }

        /// <summary>
        /// 플레이어 감지
        /// </summary>
        private void SearchTarget()
        {
            int layerMask = _owner.GetPlayerLayerMask();

            Collider[] hits = Physics.OverlapSphere(_owner.transform.position, _owner.Data.detectRange, layerMask);

            if (hits.Length > 0)
            {
                _owner.Context.target = hits[0].gameObject;
                _owner.ChangeState(EnemyStateType.Move);
            }
        }

        /// <summary>
        /// 타겟과의 거리를 계산후 판단
        /// </summary>
        private void TargetFind()
        {
            float distance = _owner.GetTargetDistance();

            if (distance <= _owner.Data.attackRange)
            {
                _owner.ChangeState(EnemyStateType.Attack);
            }
            else if (_timer >= IDLE_DURATION)
            {
                _owner.ChangeState(EnemyStateType.Move);
            }
        }

        public void Exit() { }
    }
}
