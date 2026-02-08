using Game.Project.Data.Stat;
using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Rune;
using Game.Project.Scripts.Core.Projectile.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.Project.Scripts.Managers.Systems.StateSystem;

namespace Game.Project.Scripts.Managers.Systems.SkillSystems
{
    /// <summary>
    /// 스킬 수치 연산 담당
    /// </summary>
    public class ModifierSystem : MonoBehaviour
    {
        public void ApplyModifiers(ProjectileContext context, Stat playerStat)
        {
            SkillData so = context.data;
            if (so == null) return;

            float skillFinalDamage = so.damage;
            float skillFinalSpeed = so.speed;
            float skillFinalLifeTime = so.lifeTime;
            float skillFinalScale = so.scale; 
            float skillFinalCritChance = so.critChance;
            float skillFinalCritDamage = so.critDamage;

            int skillFinalCount = so.projectileCount;

            if (so.equippedRunes != null)
            {
                foreach (var rune in so.equippedRunes)
                {
                    if (rune == null) continue;
                    skillFinalDamage *= (1f + rune.baseDamageModifier);

                    switch (rune.modifier)
                    {
                        case ModifierType.Damage:
                            skillFinalDamage *= (1f + rune.specialValue);
                            break;
                        case ModifierType.Speed:
                            skillFinalSpeed *= (1f + rune.specialValue);
                            break;
                        case ModifierType.LifeTime:
                            skillFinalLifeTime *= (1f + rune.specialValue);
                            break;
                        case ModifierType.Scale:
                            skillFinalScale *= (1f + rune.specialValue);
                            break;
                        case ModifierType.CritChance:
                            skillFinalCritChance += rune.specialValue;
                            break;
                        case ModifierType.CritDamage:
                            skillFinalCritDamage += rune.specialValue;
                            break;
                        case ModifierType.ProjectileCount:
                            skillFinalCount += (int)rune.specialValue;
                            break;
                    }
                }
            }
            context.finalDamage = playerStat.damage + skillFinalDamage;
            context.finalSpeed = (playerStat.maxMoveSpeed * 0.1f) + skillFinalSpeed;
            context.finalCritChance = playerStat.critChance + skillFinalCritChance;
            context.finalCritDamage = playerStat.critDamage + skillFinalCritDamage;

            context.finalLifeTime = skillFinalLifeTime;
            context.finalRange = so.range;
            context.finalScale = skillFinalScale;

            context.finalProjectileCount = Mathf.Max(1, skillFinalCount);

            context.isCritical = Random.value < context.finalCritChance;

            context.flyEffect = so.flyEffect;
            context.impactEffect = so.impactEffect;
            context.impactSfx = so.impactSfx;
        }
    }
}
