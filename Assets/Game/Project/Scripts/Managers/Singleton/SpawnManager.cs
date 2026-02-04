using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Project.Utillity.Generic;

namespace Game.Project.Scripts.Managers.Singleton
{
    /// <summary>
    /// 맵이나 적 스폰 담당 매니저
    /// </summary>
    public class SpawnManager : Singleton<SpawnManager>
    {
        private bool _isInitialized = false;

        public void Init()
        {
            if (_isInitialized) return;
            //맵, 스폰 위치 초기화 내용 담당
            Debug.Log("SpawnManager: 초기화 완료");

            _isInitialized = true;
        }
    }
}
