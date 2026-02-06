using Game.Project.Scripts.Core.Projectile.Rune;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.SO
{
    public enum MovementType { Linear, Growing, AreaFall, Spread }
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

        [SerializeField] public List<RuneData> equippedRunes = new List<RuneData>();

        public float damage = 10f;
        public float speed = 15f;
        public float range = 20f;
        public float lifeTime = 3;
        public float cooldown = 1.0f;
        public float explosionRadius = 0;
        public int projectileCount = 3;

        public float chargeTime = 0.2f;

        public GameObject spawnEffect;   //1회성
        public GameObject chargeEffect;  //루프
        public GameObject flyEffect;    //루프
        public GameObject impactEffect; //1회성

        public AudioClip spawnSfx;
        public AudioClip chargeSfx;
        public AudioClip flySfx;
        public AudioClip impactSfx;
    }
}
