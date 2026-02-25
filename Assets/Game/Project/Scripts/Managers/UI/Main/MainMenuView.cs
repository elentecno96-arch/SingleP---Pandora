using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Game.Project.Scripts.Managers.UI.Main
{
    /// <summary>
    /// ∏ﬁ¿Œ æ¿¿« ∏ﬁ¿Œ πˆ∆∞ ∫‰
    /// </summary>
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private Button btnNewGame;
        [SerializeField] private Button btnLoad;
        [SerializeField] private Button btnSettings;
        [SerializeField] private Button btnExit;

        public event Action OnNewGameClicked;
        public event Action OnExitClicked;
        public event Action OnLoadGameClicked;
        public event Action OnSettingsClicked;
        private void Awake()
        {
            btnNewGame.onClick.AddListener(() => OnNewGameClicked?.Invoke());
            btnExit.onClick.AddListener(() => OnExitClicked?.Invoke());
            btnLoad.onClick.AddListener(() => OnLoadGameClicked?.Invoke());
            btnSettings.onClick.AddListener(() => OnSettingsClicked?.Invoke());
        }
        public void SetAlpha(float alpha)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = alpha;
            }
        }
        public void InitView()
        {
            SetAlpha(0f);
            SetInteractable(false);
        }

        public void SetLoadButtonActive(bool isActive)
        {
            if (btnLoad != null)
            {
                btnLoad.gameObject.SetActive(isActive);
            }
        }

        public void SetInteractable(bool state)
        {
            if (canvasGroup != null)
            {
                canvasGroup.interactable = state;
                canvasGroup.blocksRaycasts = state;
            }
        }
    }
}
