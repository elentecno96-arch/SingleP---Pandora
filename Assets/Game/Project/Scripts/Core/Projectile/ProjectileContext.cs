using UnityEngine;
using Game.Project.Scripts.Core.Projectile.SO;

namespace Game.Project.Scripts.Core.Projectile
{
    /// <summary>
    /// 스킬의 정보를 전달하는 객체
    /// </summary>
    public class ProjectileContext
    {
        public SkillData Data;
        public Vector3 Direction;
        public GameObject Owner;
        public GameObject Target;
        public Projectile Projectile;

        public System.Action OnSpawnEnter;
        public System.Action OnSpawnExit;

        public System.Action OnChargeEnter;
        public System.Action<float> OnChargeUpdate;
        public System.Action OnChargeExit;

        public System.Action OnFlyEnter;
        public System.Action<Projectile> OnFlyUpdate;
        public System.Action OnFlyExit;

        public System.Action<GameObject> OnImpactEnter;

        public void ClearEvents()
        {
            OnSpawnEnter = null;
            OnSpawnExit = null;
            OnChargeEnter = null;
            OnChargeUpdate = null;
            OnChargeExit = null;
            OnFlyEnter = null;
            OnFlyUpdate = null;
            OnFlyExit = null;
            OnImpactEnter = null;
        }
    }
}
