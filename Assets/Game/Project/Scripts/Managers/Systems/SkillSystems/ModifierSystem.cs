using Game.Project.Data.Stat;
using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Rune;
using Game.Project.Scripts.Core.Projectile.SO;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems.SkillSystems
{
    /// <summary>
    /// 스킬 수치 연산 담당
    /// </summary>
    public class ModifierSystem : MonoBehaviour
    {
        public void ApplyModifiers(ProjectileContext context, List<RuneData> runes, IStatSourceable statSource)
        {
            if (context == null || context.data == null || statSource == null) return;

            Stat sourceStat = statSource.GetCurrentStat();
            SkillData so = context.data;

            float skillFinalDamage = so.damage;
            float skillFinalSpeed = so.speed;
            float skillFinalLifeTime = so.lifeTime;
            float skillFinalScale = so.scale;
            float skillFinalCritChance = so.critChance;
            float skillFinalCritDamage = so.critDamage;
            float skillFinalCooldown = so.cooldown;
            int skillFinalCount = so.projectileCount;

            float runeCDR = 0f;

            // 몬스터는 룬이 없기 떄문에 (룬이 있을 경우에만 순회)
            if (runes != null && runes.Count > 0)
            {
                foreach (var rune in runes)
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
                            skillFinalCount += Mathf.RoundToInt(rune.specialValue);
                            break;
                        case ModifierType.Cooldown:
                            runeCDR += rune.specialValue;
                            break;
                    }
                }
            }
            context.finalDamage = sourceStat.damage + skillFinalDamage;

            context.finalSpeed = (sourceStat.maxMoveSpeed * 0.1f) + skillFinalSpeed;

            context.finalCritChance = sourceStat.critChance + skillFinalCritChance;
            context.finalCritDamage = sourceStat.critDamage + skillFinalCritDamage;

            float cdAfterRune = skillFinalCooldown * (1f - runeCDR);
            float castingSpeedMod = Mathf.Max(0.1f, 1.0f + sourceStat.castingSpeed);
            float finalCD = cdAfterRune / castingSpeedMod;

            context.finalCooldown = Mathf.Max(0.05f, finalCD);

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
