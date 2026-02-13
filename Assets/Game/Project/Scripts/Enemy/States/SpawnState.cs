using Game.Project.Scripts.Enemy.Interface;
using Game.Project.Scripts.Managers.Singleton;
using UnityEngine;

namespace Game.Project.Scripts.Enemy.States
{
    /// <summary>
    /// 적 스폰 초기 상태 초기화
    /// </summary>
    public class SpawnState : IEnemyState
    {
        private readonly Enemy _owner;

        public SpawnState(Enemy owner) => _owner = owner;

        public void Enter()
        {
            var data = _owner.Context.data;
            if (data?.spawnEffect != null)
            {
                EffectManager.Instance.PlayEffect(data.spawnEffect, _owner.transform.position, Quaternion.identity);
            }
            _owner.StopMoving();
            _owner.SetMoveAnim(0f);

            _owner.ChangeState(EnemyStateType.Idle);
        }

        public void Execute() { }

        public void Exit() { }
    }
}
