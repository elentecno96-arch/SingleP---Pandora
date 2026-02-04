using Game.Project.Data.Damage;
using Game.Project.Scripts.Core.Projectile.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.States
{
    /// <summary>
    /// 투사체 충돌 상태
    /// </summary>
    public class ImpactState : IProjectileState
    {
        public void Enter(ProjectileContext context)
        {
            if (context.Target != null && context.Target.TryGetComponent(out IDamageable dmg))
            {
                dmg.TakeDamage(context.Data.damage);
            }
            context.OnImpactEnter?.Invoke(context.Target);
            context.Projectile.ReturnToPool();
        }

        public void UpdateState(Projectile projectile) { }
        public void Exit(ProjectileContext context) { }
    }
}
