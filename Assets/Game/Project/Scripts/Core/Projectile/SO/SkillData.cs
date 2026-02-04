using System.Collections;
using System.Collections.Generic;
using Game.Project.Scripts.Core.Projectile.Rune.Enum;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.SO
{
    /// <summary>
    /// 스킬 기본 정보를 담은 SO
    /// </summary>
    [CreateAssetMenu(fileName = "New Skill Data")]
    public class SkillData : ScriptableObject
    {
        public string skillName;
        public GameObject projectilePrefab;

        public float damage = 10f;
        public float speed = 15f;
        public float range = 20f;
        public float cooldown = 1.0f;

        public float chargeTime = 0.2f;

        public GameObject spawnEffect;   //1회성
        public GameObject chargeEffect;  //루프
        public GameObject flyEffect;    //루프
        public GameObject impactEffect; //1회성

        public AudioClip spawnSfx;
        public AudioClip chargeSfx;
        public AudioClip flySfx;
        public AudioClip impactSfx;

        public int projectilePoolIndex;

        //기본 속성은 무 속성이며 룬에 의해 변경됨
        public SkillAttribute attribute = SkillAttribute.None;
    }
}
