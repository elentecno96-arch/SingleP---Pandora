using Game.Project.Scripts.Core.Projectile.SO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Game.Project.Scripts.Core.Projectile.Rune
{
    public class SkillRuneOperation
    {
        private readonly List<SynergyData> _synergyDatabase;

        public SkillRuneOperation(List<SynergyData> synergyDatabase)
        {
            _synergyDatabase = synergyDatabase;
        }

        public void ApplyRunesAndSynergies(ProjectileContext context)
        {
            if (context.data.equippedRunes == null) return;

            context.skillDamage = context.data.damage;
            context.skillSpeed = context.data.speed;
            context.skillLifeTime = 4f;
            context.skillCooldown = context.data.cooldown;
            context.skillScale = 1f;
            context.skillCritChance = 0.05f;
            context.skillCritDamage = 1.5f;

            ApplyRuneEffects(context);
            ApplySynergies(context);
        }

        private void ApplyRuneEffects(ProjectileContext context)
        {
            foreach (var rune in context.data.equippedRunes)
            {
                if (rune == null) continue;
                context.skillDamage *= (1f + rune.damageMultiplier);

                switch (rune.modifier)
                {
                    case ModifierType.Damage: context.skillDamage *= (1f + rune.specialValue); break;
                    case ModifierType.Speed: context.skillSpeed *= (1f + rune.specialValue); break;
                    case ModifierType.Duration: context.skillLifeTime *= (1f + rune.specialValue); break;
                    case ModifierType.Cooldown: context.skillCooldown *= (1f - rune.specialValue); break;
                    case ModifierType.Scale: context.skillScale *= (1f + rune.specialValue); break;
                    case ModifierType.CritChance: context.skillCritChance += rune.specialValue; break;
                    case ModifierType.CritDamage: context.skillCritDamage += rune.specialValue; break;
                    case ModifierType.Acceleration: context.skillAcceleration += rune.specialValue; break;
                    case ModifierType.Homing: context.skillHomingForce += rune.specialValue; break;

                    case ModifierType.Radius: context.synergyExplosionRadius *= (1f + rune.specialValue); break;
                }
            }
            context.isCritical = UnityEngine.Random.value < context.skillCritChance;
        }

        private void ApplySynergies(ProjectileContext context)
        {
            var elementCounts = context.data.equippedRunes
                .Where(r => r != null && r.element != RuneElement.None)
                .GroupBy(r => r.element)
                .ToDictionary(g => g.Key, g => g.Count());

            var activeSynergy = _synergyDatabase
                .OrderByDescending(s => s.requiredCount)
                .FirstOrDefault(s => elementCounts.ContainsKey(s.element) && elementCounts[s.element] >= s.requiredCount);

            if (activeSynergy != null)
            {
                context.skillDamage *= (1f + activeSynergy.damageMultiplierBonus);
                context.activeSynergy = activeSynergy;

                context.primaryColor = activeSynergy.flyPalette.primary;
                context.secondaryColor = activeSynergy.flyPalette.secondary;
            }
        }
    }
}
