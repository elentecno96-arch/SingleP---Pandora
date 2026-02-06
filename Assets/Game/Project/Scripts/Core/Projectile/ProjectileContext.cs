using DG.Tweening;
using Game.Project.Scripts.Core.Projectile.Interface;
using Game.Project.Scripts.Core.Projectile.Rune;
using Game.Project.Scripts.Core.Projectile.SO;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile
{
    /// <summary>
    /// 스킬의 정보를 전달하는 객체
    /// </summary>
    public class ProjectileContext
    {
        public SkillData data;
        public Projectile projectile;
        public GameObject owner;
        public GameObject target;

        public List<IProjectileStrategy> strategies = new List<IProjectileStrategy>();
        public SynergyData activeSynergy;

        public float skillDamage;
        public float skillSpeed;
        public float skillLifeTime;
        public float skillCooldown;
        public float skillScale = 1f;
        public float skillCritChance = 0.05f;
        public float skillCritDamage = 1.5f;
        public float skillAcceleration = 0f;
        public float skillHomingForce = 0f;

        public bool isCritical; //크리 발생 여부 체크

        public float synergyExplosionRadius = 3f;
        public float synergySlowAmount = 0f;
        public float synergyDefensePen = 0f;

        public Vector3 firePosition;
        public Vector3 direction;
        public Color primaryColor = Color.white;
        public Color secondaryColor = Color.gray;
    }
}
