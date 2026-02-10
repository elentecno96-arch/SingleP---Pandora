using UnityEngine;
using Game.Project.Utility.Generic;

namespace Game.Project.Scripts.Managers.Singleton
{
    /// <summary>
    /// 씬 이동 관리 매니저
    /// </summary>
    public class SceneManager : Singleton<SceneManager>
    {
        private bool _isInitialized = false;

        public void Init()
        {
            if (_isInitialized) return;


            _isInitialized = true;
            Debug.Log("SceneManager: 초기화 완료");
        }
        public void LoadScene(string sceneName)
        {
            Debug.Log($"SceneManager: {sceneName} 로딩 시도");
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
