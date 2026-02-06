using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Rune;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems.SkillSystems
{
    /// <summary>
    /// 속성 체크, 시너지 조건 확인 담당
    /// </summary>
    public class SynergySystem
    {
        [SerializeField] private List<SynergyData> synergyDatabase;

        public SynergySystem(List<SynergyData> database)
        {
            synergyDatabase = database;
        }
        public void Init(List<SynergyData> database) => synergyDatabase = database;

        public void CheckSynergy(ProjectileContext context)
        {
            var runes = context.data.equippedRunes;
            if (runes == null || runes.Count == 0) return;

            var counts = new Dictionary<RuneElement, int>();
            foreach (var rune in runes)
            {
                if (rune == null || rune.element == RuneElement.None) continue;
                if (!counts.ContainsKey(rune.element)) counts[rune.element] = 0;
                counts[rune.element]++;
            }
            foreach (var synergy in synergyDatabase)
            {
                if (counts.TryGetValue(synergy.element, out int count))
                {
                    int activeThreshold = 0;
                    if (count >= 6) activeThreshold = 6;
                    else if (count >= 4) activeThreshold = 4;
                    else if (count >= 2) activeThreshold = 2;

                    if (activeThreshold > 0 && synergy.requiredCount == activeThreshold)
                    {
                        ApplySynergyEffects(context, synergy);
                        return;
                    }
                }
            }
        }

        private void ApplySynergyEffects(ProjectileContext context, SynergyData synergy)
        {
            context.skillDamage *= (1f + synergy.damageMultiplierBonus);

            context.activeSynergy = synergy;
            context.primaryColor = synergy.flyPalette.primary;
            context.secondaryColor = synergy.flyPalette.secondary;

            Debug.Log($"Synergy Active: {synergy.element} (Level: {synergy.requiredCount})");
        }
    }
}
