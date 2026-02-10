using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Project.Scripts.Managers.UI.Main
{
    /// <summary>
    /// 설정의 버튼 UI 뷰
    /// </summary>
    public class SettingsView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Button btnClose;

        public event Action OnCloseClicked;
        public event Action<string, float> OnVolumeChanged;

        private void Awake()
        {
            btnClose.onClick.AddListener(() => OnCloseClicked?.Invoke());

            masterSlider.onValueChanged.AddListener(v => OnVolumeChanged?.Invoke("MasterVol", v));
            bgmSlider.onValueChanged.AddListener(v => OnVolumeChanged?.Invoke("BGMVol", v));
            sfxSlider.onValueChanged.AddListener(v => OnVolumeChanged?.Invoke("SFXVol", v));

            LoadSavedVolume();
            Show(false);
        }
        private void LoadSavedVolume()
        {
            masterSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterVol", 0.8f));
            bgmSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("BGMVol", 0.8f));
            sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SFXVol", 0.8f));
        }
        public void Show(bool state)
        {
            canvasGroup.alpha = state ? 1f : 0f;
            canvasGroup.interactable = state;
            canvasGroup.blocksRaycasts = state;
        }
    }
}
