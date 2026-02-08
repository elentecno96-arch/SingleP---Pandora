using Game.Project.Scripts.Core.Projectile.Interface;

namespace Game.Project.Scripts.Core.Projectile.States
{
    /// <summary>
    /// 투사체 충돌 상태
    /// </summary>
    public class ImpactState : IProjectileState
    {
        public void Enter(ProjectileContext context, Projectile projectile)
        {
            projectile.ImpactStateCall(context.target);
            projectile.ReturnToPool();
        }

        public void UpdateState(Projectile projectile) { }
        public void Exit(ProjectileContext context) { }
    }
}
