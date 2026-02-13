using Game.Project.Scripts.Enemy.Interface;
using UnityEngine;

namespace Game.Project.Scripts.Enemy.States
{
    /// <summary>
    /// 이동 및 거리와 겹침 방지를 처리하는 상태
    /// </summary>
    public class MoveState : IEnemyState
    {
        private readonly Enemy _owner;

        private const float ATTACK_RANGE_BUFFER = 0.9f;

        public MoveState(Enemy owner) => _owner = owner;

        public void Enter()
        {
            _owner.SetMoveAnim(1.0f);
        }

        public void Execute()
        {
            var target = _owner.Context.target; //타겟 확인
            if (target == null)
            {
                _owner.ChangeState(EnemyStateType.Idle);
                return;
            }

            float distance = _owner.GetTargetDistance(); //거리 계산

            if (distance <= _owner.Data.attackRange * ATTACK_RANGE_BUFFER)
            {
                _owner.ChangeState(EnemyStateType.Attack);
                return;
            }

            Vector3 myPos = _owner.transform.position;
            Vector3 targetPos = target.transform.position;

            Vector3 moveDir = (targetPos - myPos);
            moveDir.y = 0;
            moveDir.Normalize();

            Vector3 separationDir = _owner.GetSeparationVector();

            Vector3 finalDir = (moveDir + separationDir * 0.5f).normalized;

            float moveSpeed = _owner.Context.currentStat.maxMoveSpeed;
            _owner.Move(myPos + finalDir, moveSpeed);
        }

        public void Exit()
        {
            _owner.SetMoveAnim(0.0f);
            _owner.StopMoving();
        }
    }
}
