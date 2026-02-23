using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Game.Project.Scripts.Managers.UI.SkillBulid.View
{
    /// <summary>
    /// ÀåÂø ¿©ºÎ È®ÀÎ ÆË¾÷
    /// </summary>
    public class ConfirmPopup : MonoBehaviour
    {
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button cancelButton;

        private Action _onConfirm;

        private void Awake()
        {
            confirmButton.onClick.AddListener(OnClickConfirm);
            cancelButton.onClick.AddListener(OnClickCancel);
        }

        public void Open(Action onConfirmAction)
        {
            _onConfirm = onConfirmAction;
            gameObject.SetActive(true);
        }

        private void OnClickConfirm()
        {
            _onConfirm?.Invoke();
            Close();
        }

        private void OnClickCancel() => Close();
        private void Close() => gameObject.SetActive(false);
    }
}
