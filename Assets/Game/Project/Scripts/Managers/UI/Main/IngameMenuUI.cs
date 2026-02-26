using Game.Project.Scripts.Managers.Singleton;
using UnityEngine;

namespace Game.Project.Scripts.Managers.UI.Main
{
    /// <summary>
    /// 메인 메뉴가 아닌 인게임 안에서 사용가능한 메뉴 UI
    /// </summary>
    public class IngameMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject menuRoot;
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject optionPanel;
        [SerializeField] private GameObject confirmPopup;

        [SerializeField] private SettingsView settingsView;

        private bool _isPaused = false;

        private void Start()
        {
            if (settingsView != null)
            {
                settingsView.OnVolumeChanged += (param, value) =>
                {
                    if (AudioManager.HasInstance)
                    {
                        AudioManager.Instance.SetMixerVol(param, value);
                    }
                };
                settingsView.OnCloseClicked += CloseSubPanel;
            }
        }

        void Update()
        {
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            if (currentScene == "0. Intro" || currentScene == "6. Main")
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isPaused) Resume();
                else Pause();
            }
        }

        public void Pause()
        {
            _isPaused = true;
            Time.timeScale = 0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            menuRoot.SetActive(true);
            ShowMainPanel();
        }

        public void Resume()
        {
            _isPaused = false;
            Time.timeScale = 1f;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            menuRoot.SetActive(false);
            CloseSubPanel();
        }

        private void ShowMainPanel()
        {
            mainPanel.SetActive(true);
            optionPanel.SetActive(false);
            confirmPopup.SetActive(false);
        }

        public void OnClickOption()
        {
            mainPanel.SetActive(false);
            optionPanel.SetActive(true);
        }

        public void OnClickGoToMenu()
        {
            if (confirmPopup != null)
            {
                confirmPopup.SetActive(true);
                confirmPopup.transform.SetAsLastSibling();
            }
        }

        public void ConfirmGoToMenu()
        {
            Time.timeScale = 1f;
            _isPaused = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (confirmPopup != null) confirmPopup.SetActive(false);
            if (menuRoot != null) menuRoot.SetActive(false);

            UiManager.Instance.CloseAllSystemUI();
            AudioManager.Instance.PlayMainBgm();

            SceneManager.Instance.LoadScene("6. Main");
        }

        public void CloseSubPanel()
        {
            optionPanel.SetActive(false);
            confirmPopup.SetActive(false);
            mainPanel.SetActive(true);
        }
    }
}
