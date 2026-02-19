using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.Systems.PlayerSystems;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Project.Scripts.Managers.UI.AbilityTree
{
    /// <summary>
    /// 스탯 패널의 중개자
    /// </summary>
    public class AbilityPresenter : MonoBehaviour
    {
        [SerializeField] private AbilityTreeView _treeView;
        [SerializeField] private TextMeshProUGUI _pointText;
        [SerializeField] private Button _resetButton;

        private AbilitySystem _abilitySystem;
        private LevelSystem _levelSystem;

        [SerializeField] private GameObject _abilityPanel;
        private bool _isOpened = false;
        private bool _isLocked = false;
        public void SetLocked(bool lockState) => _isLocked = lockState;

        [SerializeField] private AbilityTooltip _tooltip;

        //플레이어 매니저가 초기화 될 때 까지 한턴 기다림
        private IEnumerator Start()
        {
            while (!PlayerManager.HasInstance)
            {
                yield return null;
            }

            yield return null;

            _abilitySystem = PlayerManager.Instance.abilitySystem;
            _levelSystem = PlayerManager.Instance.levelSystem;

            if (_abilitySystem != null && _abilitySystem.AbilityNodes != null)
            {
                if (_resetButton != null)
                {
                    _resetButton.onClick.AddListener(OnRequestReset);
                }

                _treeView.CreateTree(_abilitySystem.AbilityNodes, OnRequestUnlock);

                _treeView.OnNodeEnter = ShowTooltip;
                _treeView.OnNodeExit = HideTooltip;

                UpdatePointUI(_levelSystem.SkillPoint);
                _levelSystem.OnPointChanged += UpdatePointUI;

                ClosePanel();
            }
            else
            {
                Debug.LogError("AbilityPresenter: AbilitySystem 또는 Nodes를 찾을 수 없습니다.");
            }
        }

        private void Update()
        {
            if (_isLocked) return;

            if (Input.GetKeyDown(KeyCode.U))
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
            _abilityPanel.SetActive(true);

            _treeView.RefreshTree();
            UpdatePointUI(_levelSystem.SkillPoint);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void ClosePanel()
        {
            _isOpened = false;
            _abilityPanel.SetActive(false);
            HideTooltip();
        }

        private void OnDestroy()
        {
            if (_levelSystem != null)
            {
                _levelSystem.OnPointChanged -= UpdatePointUI;
            }
        }

        private void UpdatePointUI(int currentPoints)
        {
            if (_pointText != null)
            {
                _pointText.text = currentPoints.ToString();
            }
        }

        private void OnRequestUnlock(AbilityNote node)
        {
            bool success = _abilitySystem.TryUnlockNode(node);

            if (success)
            {
                _treeView.RefreshTree();
                ShowTooltip(node);
            }
        }

        private void OnRequestReset()
        {
            _abilitySystem.ResetAllAbilities();
            _treeView.RefreshTree();
            UpdatePointUI(_levelSystem.SkillPoint);
            HideTooltip();
        }

        private void ShowTooltip(AbilityNote node)
        {
            if (_tooltip != null && _isOpened)
            {
                _tooltip.Show(node);
            }
        }

        private void HideTooltip()
        {
            if (_tooltip != null)
            {
                _tooltip.Hide();
            }
        }
    }
}
