using Game.Project.Scripts.Data.Items;
using Game.Project.Scripts.Managers.UI.AbilityTree;
using Game.Project.Scripts.Managers.UI.Intro;
using Game.Project.Scripts.Managers.UI.ItemPopUp;
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
    }
}
