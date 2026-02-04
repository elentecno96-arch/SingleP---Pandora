using Game.Project.Utillity.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Interface;
using Game.Project.Scripts.Core.Projectile.SO;

namespace Game.Project.Scripts.Managers.Singleton
{
    /// <summary>
    /// 스킬 이펙트의 풀, 룬, 시너지 관리하는 매니저
    /// </summary>
    public class SkillManager : Singleton<SkillManager>
    {
        private bool _isInitialized = false;

        public void Init()
        {
            if (_isInitialized) return;
            //추후 추가될 룬이나, 시너지 관련 시스템 초기화 예정
            _isInitialized = true;
            Debug.Log("SkillManager 초기화 완료");
        }
        public void FireProjectile(GameObject prefab, Vector3 pos, Vector3 dir, GameObject owner, SkillData data, List<IProjectileStrategy> strategies)
        {
            if (prefab == null) return;

            Projectile proj = PoolManager.Instance.GetProjectile(prefab);
            proj.transform.SetPositionAndRotation(pos, Quaternion.LookRotation(dir));
            proj.Init(owner, dir, data, strategies);
        }
    }
}
