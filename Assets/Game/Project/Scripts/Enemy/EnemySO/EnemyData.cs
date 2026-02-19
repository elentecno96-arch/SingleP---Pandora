using Game.Project.Data.Stat;
using Game.Project.Scripts.Core.Projectile.SO;
using UnityEngine;

namespace Game.Project.Scripts.Enemy.EnemySO
{
    /// <summary>
    /// 적 캐릭터의 정적 데이터
    /// </summary>
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "Data/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [Header("Basic Info")]
        public string enemyName;
        public GameObject prefab;

        public Stat baseStat;           // 기초 스탯
        public SkillData skillData;     // 스킬 데이터

        public float detectRange = 10f;
        public float attackRange = 2f;

        public int expReward = 20;            //경험치 량
        public GameObject[] lootTable;        //드랍 아이템 배열
        [Range(0, 100)] public float dropChance = 30f; //드랍 확률

        [Header("Visual Effects")]
        public GameObject spawnEffect;
        public GameObject detectEffect;
        public GameObject attackEffect;
        public GameObject hitEffect;
        public GameObject deathEffect;

        [Header("Audio Clips")]
        public AudioClip spawnSfx;
        public AudioClip detectSfx;
        public AudioClip moveSfx;
        public AudioClip attackSfx;
        public AudioClip hitSfx;
        public AudioClip deathSfx;
    }
}
