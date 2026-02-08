using Game.Project.Scripts.Core.Projectile.SO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Game.Project.Scripts.Core.Projectile.Rune
{
    public class SkillRuneOperation
    {
        public void ApplyRunes(ProjectileContext context)
        {
            if (context.data == null) return;

            context.finalDamage = context.data.damage;
            context.finalSpeed = context.data.speed;
            context.finalLifeTime = context.data.lifeTime;
            context.finalScale = 1f;
            context.finalCritChance = 0.05f;
            context.finalCritDamage = 1.5f;

            context.finalProjectileCount = context.data.projectileCount;

            if (context.data.equippedRunes != null)
            {
                ApplyRuneEffects(context);
            }
            context.isCritical = UnityEngine.Random.value < context.finalCritChance;
        }

        private void ApplyRuneEffects(ProjectileContext context)
        {
            foreach (var rune in context.data.equippedRunes)
            {
                if (rune == null) continue;

                context.finalDamage *= (1f + rune.baseDamageModifier);

                switch (rune.modifier)
                {
                    case ModifierType.Damage:
                        context.finalDamage *= (1f + rune.specialValue); break;
                    case ModifierType.Speed:
                        context.finalSpeed *= (1f + rune.specialValue); break;
                    case ModifierType.LifeTime:
                        context.finalLifeTime *= (1f + rune.specialValue); break;
                    case ModifierType.Scale:
                        context.finalScale *= (1f + rune.specialValue); break;
                    case ModifierType.CritChance:
                        context.finalCritChance += rune.specialValue; break;
                    case ModifierType.CritDamage:
                        context.finalCritDamage += rune.specialValue; break;
                    case ModifierType.ProjectileCount:
                        context.finalProjectileCount += (int)rune.specialValue; break;
                }
            }
        }
    }
}
