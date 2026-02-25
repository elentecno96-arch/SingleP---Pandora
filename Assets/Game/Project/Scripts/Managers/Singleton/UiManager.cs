using Game.Project.Scripts.Data.Items;
using Game.Project.Scripts.Managers.UI.AbilityTree;
using Game.Project.Scripts.Managers.UI.Intro;
using Game.Project.Scripts.Managers.UI.ItemPopUp;
using Game.Project.Scripts.Player;
using Game.Project.Scripts.Managers.UI.SkillBulid;
using Game.Project.Scripts.Managers.UI.StatInfo;
using Game.Project.Utility.Generic;
using UnityEngine;

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

        //===================================================== //위의 내용은 추후 분리될 예정

        [SerializeField] private CombatHUD combatHUD;
        public CombatHUD GetCombatHUD() => combatHUD;

        [SerializeField] private SkillBuilderMediator skillBuildMediator;
        public SkillBuilderMediator SkillBuild => skillBuildMediator;
        [SerializeField] private AbilityPresenter abilityPresenter;
        public AbilityPresenter AbilityTree => abilityPresenter;
        [SerializeField] public PlayerStatPresenter playerStatPresenter;
        public PlayerStatPresenter PlayerStat => playerStatPresenter;

        [SerializeField] private GameObject duopItemsInfoPrefab;
        [SerializeField] private Transform acquisitionParent;


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
        public void ToggleSkillBuild()
        {
            if (skillBuildMediator != null)
            {
                skillBuildMediator.ToggleSkillBuild();
            }
        }

        public void ShowAcquisitionPopup(ItemData item, int amount)
        {
            if (duopItemsInfoPrefab == null || acquisitionParent == null) return;

            var popup = PoolManager.Instance.GetItemPopup(duopItemsInfoPrefab, acquisitionParent);
            popup.Setup(item, amount);
        }

        /// <summary>
        /// 모든 시스템 UI 창을 강제로 닫고 HUD를 숨깁니다.
        /// </summary>
        public void CloseAllSystemUI()
        {
            if (skillBuildMediator != null) skillBuildMediator.gameObject.SetActive(false);
            if (abilityPresenter != null) abilityPresenter.gameObject.SetActive(false);
            if (playerStatPresenter != null) playerStatPresenter.gameObject.SetActive(false);

            HideCombatHUD();
        }

        public void UnlockFullSystem()
        {
            ShowCombatHUD();

            if (skillBuildMediator != null)
            {
                skillBuildMediator.gameObject.SetActive(true);
                skillBuildMediator.IsLocked = false;
            }

            if (abilityPresenter != null)
            {
                abilityPresenter.gameObject.SetActive(true);
                abilityPresenter.SetLocked(false);
            }

            if (playerStatPresenter != null)
            {
                playerStatPresenter.gameObject.SetActive(true);
                playerStatPresenter.SetLocked(false);
            }

            Debug.Log("<color=cyan>UiManager: 던전 직행을 위한 모든 시스템 해금 완료</color>");
        }

        public void HideCombatHUD()
        {
            if (combatHUD != null) combatHUD.gameObject.SetActive(false);
        }

        public void ShowCombatHUD()
        {
            if (combatHUD != null) combatHUD.gameObject.SetActive(true);
        }
    }
}
