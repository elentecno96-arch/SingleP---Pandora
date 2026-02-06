using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.Rune
{
    [System.Serializable]
    public struct ColorPalette
    {
        public Color primary;
        public Color secondary;
        public Color tertiary;
    }

    [CreateAssetMenu(fileName = "NewSynergy", menuName = "Projectile/Synergy Data")]
    public class SynergyData : ScriptableObject
    {
        public string synergyName;
        public RuneElement element;
        public int requiredCount;

        [Header("Bonus Stats")]
        public float damageMultiplierBonus;

        [Header("Visual Palettes")]
        public ColorPalette spawnPalette;
        public ColorPalette chargePalette;
        public ColorPalette flyPalette;
        public ColorPalette impactPalette;
    }
}
