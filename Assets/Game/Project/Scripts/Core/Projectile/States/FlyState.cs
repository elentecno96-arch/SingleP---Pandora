using UnityEngine;
using Game.Project.Scripts.Core.Projectile.Interface;

namespace Game.Project.Scripts.Core.Projectile.States
{
    /// <summary>
    /// 투사체가 이동하는 상태
    /// </summary>
    public class FlyState : IProjectileState
    {
        public void Enter(ProjectileContext context, Projectile projectile)
        {
            projectile.transform.localScale = Vector3.one * context.finalScale;
        }

        public void UpdateState(Projectile projectile)
        {
            projectile.StateTimer += Time.deltaTime;

            float maxLifeTime = projectile.Context.finalLifeTime > 0
                ? projectile.Context.finalLifeTime : 4f;

            if (projectile.StateTimer >= maxLifeTime)
            {
                projectile.ReturnToPool();
                return;
            }
            projectile.Mover?.OnUpdate(projectile);
        }
        public void Exit(ProjectileContext context) { }
    }
}
