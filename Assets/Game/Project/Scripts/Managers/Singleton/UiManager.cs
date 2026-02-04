using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Project.Utillity.Generic;

namespace Game.Project.Scripts.Managers.Singleton
{
    /// <summary>
    /// 전체UI 담당하는 매니저
    /// </summary>
    public class UiManager : Singleton<UiManager>
    {
        private bool _isInitialized = false;

        public void Init()
        {
            if (_isInitialized) return;
            //추후 UI관련 초기화 내용 담당
            _isInitialized = true;
            Debug.Log("UiManager: 초기화 완료");
        }
    }
}
