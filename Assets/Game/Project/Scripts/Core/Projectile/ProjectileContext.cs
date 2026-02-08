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
        public GameObject owner;
        public GameObject target;

        public float finalDamage;
        public float finalSpeed;
        public float finalLifeTime;
        public float finalRange;
        public float finalScale = 1f; 
        public float finalCritChance;
        public float finalCritDamage;
        public int finalProjectileCount;

        public bool isCritical;

        public GameObject flyEffect;
        public GameObject impactEffect;
        public AudioClip impactSfx;

        public Vector3 firePosition;
        public Vector3 direction;

        public ProjectileContext Clone()
        {
            return (ProjectileContext)this.MemberwiseClone();
        }
    }
}
