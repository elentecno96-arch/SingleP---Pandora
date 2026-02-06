using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Rune;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems.SkillSystems
{
    /// <summary>
    /// 스킬 수치 연산 담당
    /// </summary>
    public class ModifierSystem : MonoBehaviour
    {
        public void InitContextStats(ProjectileContext context)
        {
            var data = context.data;
            context.skillDamage = data.damage;
            context.skillSpeed = data.speed;
            context.skillLifeTime = 4f;
            context.skillCooldown = data.cooldown;
            context.skillScale = 1f;
            context.skillCritChance = 0.05f;
            context.skillCritDamage = 1.5f;
            context.skillAcceleration = 0f;
            context.skillHomingForce = 0f;

            context.synergyExplosionRadius = data.explosionRadius;
            context.synergySlowAmount = 0f;
            context.synergyDefensePen = 0f;
        }

        public void ApplyRunes(ProjectileContext context)
        {
            var runes = context.data.equippedRunes;
            if (runes == null || runes.Count == 0) return;

            int maxSlots = (int)context.data.rarity;
            for (int i = 0; i < Mathf.Min(runes.Count, maxSlots); i++)
            {
                if (runes[i] == null) continue;

                context.skillDamage *= (1f + runes[i].damageMultiplier);
                SingleRune(context, runes[i]);
            }
            context.isCritical = UnityEngine.Random.value < context.skillCritChance;
        }

        private void SingleRune(ProjectileContext context, RuneData rune)
        {
            switch (rune.modifier)
            {
                case ModifierType.Speed:
                    context.skillSpeed *= (1f + rune.specialValue);
                    break;
                case ModifierType.Duration:
                    context.skillLifeTime *= (1f + rune.specialValue);
                    break;
                case ModifierType.Cooldown:
                    context.skillCooldown *= (1f - rune.specialValue);
                    break;
                case ModifierType.Scale:
                    context.skillScale *= (1f + rune.specialValue);
                    break;
                case ModifierType.CritChance:
                    context.skillCritChance += rune.specialValue;
                    break;
                case ModifierType.CritDamage:
                    context.skillCritDamage += rune.specialValue;
                    break;
                case ModifierType.Acceleration:
                    context.skillAcceleration += rune.specialValue;
                    break;
                case ModifierType.Homing:
                    context.skillHomingForce += rune.specialValue;
                    break;
                case ModifierType.Radius:
                    context.synergyExplosionRadius *= (1f + rune.specialValue);
                    break;
            }
        }
    }
}
