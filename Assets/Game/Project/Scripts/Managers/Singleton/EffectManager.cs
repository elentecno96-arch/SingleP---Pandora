using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Project.Utillity.Generic;

namespace Game.Project.Scripts.Managers.Singleton
{
    /// <summary>
    /// VFX의 관리를 담당하는 전역 매니저
    /// </summary>
    public class EffectManager : Singleton<EffectManager>
    {
        private bool _isInitialized = false;

        public void Init()
        {
            if (_isInitialized) return;

            Debug.Log("EffectManager: 초기화 완료");

            _isInitialized = true;
        }
        public void PlayEffect(GameObject prefab, Vector3 pos, Quaternion rot, float duration = 3f, Transform parent = null)
        {
            if (!_isInitialized || prefab == null) return;

            GameObject fx = PoolManager.Instance.GetGameObject(prefab, parent);
            fx.transform.SetPositionAndRotation(pos, rot);

            if (duration > 0)
                StartCoroutine(CoReturn(fx, duration));
        }

        private IEnumerator CoReturn(GameObject fx, float time)
        {
            yield return new WaitForSeconds(time);
            if (fx != null && fx.activeSelf)
            {
                PoolManager.Instance.ReleaseGameObject(fx);
            }
        }
    }
}