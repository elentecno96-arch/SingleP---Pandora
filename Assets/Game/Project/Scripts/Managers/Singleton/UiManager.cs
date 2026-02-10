using UnityEngine;
using Game.Project.Utility.Generic;
using Game.Project.Scripts.Managers.UI.Intro;

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

        [SerializeField] private BlackScreenView blackScreen;
        public BlackScreenView BlackScreen => blackScreen;

        public void Init()
        {
            if (_isInitialized) return;

            if (staticRoot != null) staticRoot.SetActive(true);
            if (unstaticRoot != null) unstaticRoot.SetActive(true);

            if (blackScreen != null) blackScreen.SetAlpha(1f);

            _isInitialized = true;
            Debug.Log("UiManager: 초기화 완료");
        }
        public void ShowMainMenu()
        {

        }
    }
}
