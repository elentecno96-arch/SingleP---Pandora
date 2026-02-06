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

        public void Enter(ProjectileContext context)
        {
            _flyTimer = 0f;

            if (context.projectile != null)
            {
                context.projectile.transform.localScale = Vector3.one * context.skillScale;
            }
        }

        public void UpdateState(Projectile projectile)
        {
            _flyTimer += Time.deltaTime;

            float maxLifeTime = projectile.Context.skillLifeTime > 0
                ? projectile.Context.skillLifeTime : 4f;

            if (_flyTimer >= maxLifeTime)
            {
                projectile.ReturnToPool();
                return;
            }
            projectile.Mover?.OnUpdate(projectile);
        }
        public void Exit(ProjectileContext context) { }
    }
}
