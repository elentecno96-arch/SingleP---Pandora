using Game.Project.Scripts.Enemy.Interface;
using Game.Project.Scripts.Managers.Singleton;
using UnityEngine;

namespace Game.Project.Scripts.Enemy.States
{
    /// <summary>
    /// 죽음 연출 후 오브젝트 풀 반환
    /// </summary>
    public class DeadState : IEnemyState
    {
        private readonly Enemy _owner;
        private float _timer;
        private const float RETURN_DELAY = 2.0f; 

        public DeadState(Enemy owner)
        {
            _owner = owner;
        }

        public void Enter()
        {
            _timer = 0f;
            _owner.OnDead();
        }

        public void Execute()
        {
            _timer += Time.deltaTime;

            if (_timer >= RETURN_DELAY)
            {
                ReturnToPool();
            }
        }

        private void ReturnToPool()
        {
            _timer = -100f;

            if (PoolManager.Instance != null)
            {
                PoolManager.Instance.ReturnEnemy(_owner);
            }
            else
            {
                _owner.gameObject.SetActive(false);
            }
        }

        public void Exit() { }
    }
}
