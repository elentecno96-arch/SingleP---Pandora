using Game.Project.Data.Damage;
using Game.Project.Scripts.Core.Projectile.Interface;
using Game.Project.Scripts.Core.Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.Rune.RuneStrategy
{
    /// <summary>
    /// 테스트용 불 속성 추가 룬
    /// </summary>
    public class FireRuneStrategy : IProjectileStrategy
    {
        public void Apply(ProjectileContext context)
        {
            context.OnImpactEnter += (target) =>
            {
                if (target.TryGetComponent(out IDamageable dmg))
                {
                    dmg.TakeDamage(context.Data.damage * 0.2f);
                    Debug.Log($"{target.name}에게 화염 속성 타격!");
                }
            };
        }
    }
}
