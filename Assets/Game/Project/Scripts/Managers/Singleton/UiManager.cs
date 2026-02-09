using UnityEngine;
using Game.Project.Utility.Generic;

namespace Game.Project.Scripts.Managers.Singleton
{
    /// <summary>
    /// 전체UI 담당하는 매니저
    /// </summary>
    public class UiManager : Singleton<UiManager>
    {
        private bool _isInitialized = false;

        [SerializeField] private GameObject staticRoot;
        [SerializeField] private GameObject unstaticRoot;

        [SerializeField] private GameObject mainMenuPanel;

        public void Init()
        {
            if (_isInitialized) return;

            if (staticRoot != null) staticRoot.SetActive(false);
            if (unstaticRoot != null) unstaticRoot.SetActive(true);

            _isInitialized = true;
            Debug.Log("UiManager: 초기화 완료");
        }
        public void ShowMainMenu()
        {

        }
    }
}
