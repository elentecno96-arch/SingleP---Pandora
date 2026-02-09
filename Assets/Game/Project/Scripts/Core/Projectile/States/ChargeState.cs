using Game.Project.Scripts.Core.Projectile.Interface;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.States
{
    /// <summary>
    /// 스킬을 모으는 상태
    /// </summary>
    public class ChargeState : IProjectileState
    {
        public void Enter(ProjectileContext context, Projectile projectile) { }
        public void UpdateState(Projectile projectile)
        {
            projectile.StateTimer += Time.deltaTime;

            if (projectile.StateTimer >= projectile.Context.data.chargeTime)
            {
                projectile.ChangeState(ProjectileStates.Fly);
            }
        }
        public void Exit(ProjectileContext context) { }
    }
}
