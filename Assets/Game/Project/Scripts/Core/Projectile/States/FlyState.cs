using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Project.Scripts.Core.Projectile.Interface;

namespace Game.Project.Scripts.Core.Projectile.States
{
    /// <summary>
    /// 투사체가 이동하는 상태
    /// </summary>
    public class FlyState : IProjectileState
    {
        private float _flyTimer = 0f;
        private const float MAX_LIFETIME = 5f;
        public void Enter(ProjectileContext context)
        {
            _flyTimer = 0f;
            context.OnFlyEnter?.Invoke();
        }
        public void UpdateState(Projectile projectile)
        {
            _flyTimer += Time.deltaTime;
            if (_flyTimer >= MAX_LIFETIME)
            {
                projectile.ReturnToPool();
                return;
            }

            projectile.transform.position += projectile.Context.Direction * projectile.Context.Data.speed * Time.deltaTime;
            projectile.Context.OnFlyUpdate?.Invoke(projectile);
        }
        public void Exit(ProjectileContext context)
        {
            context.OnFlyExit?.Invoke();
        }
    }
}
