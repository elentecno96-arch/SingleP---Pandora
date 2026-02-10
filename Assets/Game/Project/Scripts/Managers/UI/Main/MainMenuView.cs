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
        public event Action OnSettingsClicked;
        private void Awake()
        {
            btnNewGame.onClick.AddListener(() => OnNewGameClicked?.Invoke());
            btnExit.onClick.AddListener(() => OnExitClicked?.Invoke());
            btnSettings.onClick.AddListener(() => OnSettingsClicked?.Invoke());

            if (btnLoad != null) btnLoad.interactable = false;
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
