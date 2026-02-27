using Game.Project.Scripts.Managers.Singleton;
using System.Collections;
using UnityEngine;


namespace Game.Project.Scripts.Managers.UI.StatInfo
{
    /// <summary>
    /// 플레이어 속성 UI 중개자
    /// </summary>
    public class PlayerStatPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject _statPanel;
        [SerializeField] private StatDisplayView _statView;

        private Game.Project.Scripts.Managers.Systems.PlayerSystems.StatSystem _statSystem;
        private Game.Project.Scripts.Managers.Systems.PlayerSystems.LevelSystem _levelSystem;

        private bool _isOpened = false;
        private bool _isLocked = false;
        public void SetLocked(bool lockState) => _isLocked = lockState;

        private IEnumerator Start()
        {
            while (PlayerManager.Instance == null ||
                   PlayerManager.Instance.Stats == null ||
                   PlayerManager.Instance.levelSystem == null)
            {
                yield return null;
            }

            _statSystem = PlayerManager.Instance.Stats;
            _levelSystem = PlayerManager.Instance.levelSystem;

            _statSystem.OnStatChanged += RefreshUI;

            RefreshUI();

            ClosePanel();
        }

        private void Update()
        {
            if (_isLocked) return;

            if (Input.GetKeyDown(KeyCode.P))
            {
                if (_isOpened) ClosePanel();
                else OpenPanel();
            }

            if (_isOpened && Input.GetKeyDown(KeyCode.Escape))
            {
                ClosePanel();
            }
        }

        private void OpenPanel()
        {
            _isOpened = true;
            _statPanel.SetActive(true);

            if (_statSystem == null && PlayerManager.Instance != null)
            {
                _statSystem = PlayerManager.Instance.Stats;
                if (_statSystem != null) _statSystem.OnStatChanged += RefreshUI;
            }

            RefreshUI();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void ClosePanel()
        {
            _isOpened = false;
            _statPanel.SetActive(false);
        }

        private void RefreshUI()
        {
            if (_statView != null && _statSystem != null && _levelSystem != null)
            {
                _statView.UpdateAllStats(_levelSystem.CurrentLevel, _statSystem.CurrentStat);
            }
        }

        private void OnDestroy()
        {
            if (_statSystem != null)
            {
                _statSystem.OnStatChanged -= RefreshUI;
            }
        }
    }
}
