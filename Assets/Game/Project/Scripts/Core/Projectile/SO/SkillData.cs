using Game.Project.Scripts.Core.Projectile.Rune;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.SO
{
    public enum MovementType { Linear, Growing, Rifle, Arcane, Spin, Bounce }
    public enum SkillRarity { Normal = 2, Unique = 4, Legend = 6 }
    /// <summary>
    /// 스킬 기본 정보를 담은 SO
    /// </summary>
    [CreateAssetMenu(fileName = "New Skill data")]
    public class SkillData : ScriptableObject
    {
        public string skillName;
        public GameObject projectilePrefab;
        public MovementType movementType;
        public SkillRarity rarity;
        [TextArea] public string description;

        public List<RuneData> equippedRunes = new List<RuneData>();

        public float damage = 10f;
        public float speed = 15f;
        public float range = 20f;
        public float lifeTime = 3f;
        public float cooldown = 3.0f;
        public float scale = 1.0f;        //기본 크기 필드
        public float critChance = 0.05f;  //기본 치명타 확률
        public float critDamage = 1.5f;   //기본 치명타 배율

        public float chargeTime = 0.2f;
        public int projectileCount = 1;
        public float acceleration = 0f;   //가속도
        public float homingStrength = 0f; //유도 성능
        public float areaRadius = 1.0f;   //범위

        public GameObject spawnEffect;
        public GameObject chargeEffect;
        public GameObject flyEffect;
        public GameObject impactEffect;

        public AudioClip spawnSfx;
        public AudioClip chargeSfx;
        public AudioClip flySfx;
        public AudioClip impactSfx;
    }
}
