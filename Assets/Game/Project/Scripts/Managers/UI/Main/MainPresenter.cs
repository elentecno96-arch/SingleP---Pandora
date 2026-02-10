using Game.Project.Scripts.Managers.Singleton;
using System.Collections;
using UnityEngine;
using Game.Project.Utility.Extension;

namespace Game.Project.Scripts.Managers.UI.Main
{
    /// <summary>
    /// 메인 화면 중개자
    /// </summary>
    public class MainPresenter : MonoBehaviour
    {
        [SerializeField] private MainBackgroundView backgroundView;
        [SerializeField] private MainMenuView menuView;
        [SerializeField] private SettingsView settingsView;

        private Coroutine _mainSequenceCo;
        private bool _hasPlayedIntro;

        private void Awake()
        {
            backgroundView.InitView();
            menuView.InitView();
        }

        private void OnEnable()
        {
            menuView.OnNewGameClicked += NewGame;
            menuView.OnExitClicked += ExitGame;
            menuView.OnSettingsClicked += OnSettingsOpen;

            settingsView.OnCloseClicked += OnSettingsClose;
            settingsView.OnVolumeChanged += VolumeChanged;

            if (_hasPlayedIntro) return;
            _hasPlayedIntro = true;

            _mainSequenceCo = StartCoroutine(MainSequence());
        }

        private void OnDisable()
        {
            menuView.OnNewGameClicked -= NewGame;
            menuView.OnExitClicked -= ExitGame;
            menuView.OnSettingsClicked -= OnSettingsOpen;

            settingsView.OnCloseClicked -= OnSettingsClose;
            settingsView.OnVolumeChanged -= VolumeChanged;

            if (_mainSequenceCo != null)
            {
                StopCoroutine(_mainSequenceCo);
                _mainSequenceCo = null;
            }
        }

        private IEnumerator MainSequence()
        {
            backgroundView.PlayVideo();

            if (UiManager.HasInstance && UiManager.Instance.BlackScreen != null)
            {
                yield return StartCoroutine(UiManager.Instance.BlackScreen.Group.FadeCo(1f, 0f, 1.5f));
            }

            float elapsed = 0f;
            const float duration = 1.2f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                backgroundView.SetAlpha(t);
                menuView.SetAlpha(t);

                yield return null;
            }

            backgroundView.SetAlpha(1f);
            menuView.SetAlpha(1f);
            menuView.SetInteractable(true);
        }
        private void VolumeChanged(string parameter, float value)
        {
            SetVolume(parameter, value);
        }
        private void SetVolume(string parameter, float value)
        {
            if (!AudioManager.HasInstance) return;
            AudioManager.Instance.SetMixerVol(parameter, value);
        }

        private void NewGame()
        {
            StartCoroutine(Co_NewGameSequence());
        }

        private IEnumerator Co_NewGameSequence()
        {
            menuView.SetInteractable(false);

            if (UiManager.HasInstance && UiManager.Instance.BlackScreen != null)
            {
                yield return StartCoroutine(UiManager.Instance.BlackScreen.Group.FadeCo(1f, 0f, 1.5f));
            }

            ResetProjectileData();
            // SceneManager.Instance.LoadScene("TutorialScene");
        }

        private void ResetProjectileData()
        {
            Debug.Log("ProjectileContext 데이터 초기화 완료");
        }

        private void OnSettingsOpen() => settingsView.Show(true);
        private void OnSettingsClose() => settingsView.Show(false);

        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
