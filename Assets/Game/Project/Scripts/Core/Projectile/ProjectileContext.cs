using Game.Project.Scripts.Core.Projectile.SO;
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
        public float finalCooldown;
        public int finalProjectileCount;

        public bool isCritical;

        public GameObject flyEffect;
        public GameObject impactEffect;
        public AudioClip impactSfx;

        public Vector3 firePosition;
        public Vector3 direction;

        //MemberwiseClone 얍복
        //.Net의 기능
        public ProjectileContext Clone()
        {
            return (ProjectileContext)this.MemberwiseClone();
        }
    }
}
