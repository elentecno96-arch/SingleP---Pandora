using Game.Project.Scripts.Core.Projectile.Interface;

namespace Game.Project.Scripts.Core.Projectile.States
{
    /// <summary>
    /// 투사체 시작 상태
    /// </summary>
    public class SpawnState : IProjectileState
    {
        public void Enter(ProjectileContext context, Projectile projectile) { }

        public void UpdateState(Projectile projectile)
        {
            projectile.ChangeState(ProjectileStates.Charge);
        }

        public void Exit(ProjectileContext context) { }
    }
}
