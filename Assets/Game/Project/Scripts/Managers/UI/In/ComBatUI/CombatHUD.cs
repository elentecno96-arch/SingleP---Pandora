using Game.Project.Scripts.Managers.Systems.PlayerSystems;
using Game.Project.Scripts.Managers.UI.In.ComBatUI.View;
using UnityEngine;

public class CombatHUD : MonoBehaviour
{
    /// <summary>
    /// 인게임 화면에 플레이어 세부 정보 UI
    /// </summary>
    [SerializeField] private PlayerHPUI playerHPView;
    [SerializeField] private SkillSlotUI[] skillSlotViews;
    [SerializeField] private PlayerExpUI playerExpView;

    private StateSystem _stateModel;
    private SkillEquipSystem _skillModel;
    private LevelSystem _levelModel;

    private void Update()
    {
        if (_skillModel == null) return;

        var slots = _skillModel.GetSkillSlots();
        for (int i = 0; i < skillSlotViews.Length; i++)
        {
            if (i < slots.Count && !slots[i].IsEmpty && slots[i].context != null)
            {
                slots[i].UpdateCooldown(Time.deltaTime);
                skillSlotViews[i].UpdateCooldown(slots[i].currentCooldown, slots[i].context.finalCooldown);
            }
        }
    }

    public void Bind(StateSystem model)
    {
        if (_stateModel != null) _stateModel.OnHpChanged -= OnHpChanged; // 핸들러 해제

        _stateModel = model;
        _stateModel.OnHpChanged += OnHpChanged;

        UpdateView(); 
    }

    private void OnHpChanged(float currentHp)
    {
        UpdateView();
    }

    public void BindSkills(SkillEquipSystem model)
    {
        if (_skillModel != null)
            _skillModel.OnSkillChanged -= RefreshSkillUI;

        _skillModel = model;
        _skillModel.OnSkillChanged += RefreshSkillUI;
        RefreshSkillUI();
    }

    public void BindLevel(LevelSystem model)
    {
        Debug.Log("[CombatHUD] BindLevel 호출됨! 모델 연결 시도");
        if (_levelModel != null)
        {
            _levelModel.OnExpChanged -= OnExpChanged;
            _levelModel.OnLevelUp -= OnLevelUp;
        }

        _levelModel = model;
        _levelModel.OnExpChanged += OnExpChanged;
        _levelModel.OnLevelUp += OnLevelUp;

        UpdateLevelView();
    }

    private void UpdateView()
    {
        if (playerHPView != null && _stateModel != null)
        {
            playerHPView.SetHealth(_stateModel.HpRatio);
        }
    }

    private void OnExpChanged(int level, float currentExp, float maxExp)
    {
        if (playerExpView != null)
        {
            float ratio = (maxExp > 0) ? currentExp / maxExp : 0;

            Debug.Log($"[HUD] UI 갱신 데이터 - Cur: {currentExp}, Max: {maxExp}, Ratio: {ratio}");

            playerExpView.SetExp(ratio, level);
        }
    }

    private void OnLevelUp(int newLevel)
    {
        UpdateLevelView();
        //레벨업 이펙트 추가 예정
    }

    private void UpdateLevelView()
    {
        if (playerExpView != null && _levelModel != null)
        {
            playerExpView.SetExp(_levelModel.ExpRatio, _levelModel.CurrentLevel);
        }
    }

    private void RefreshSkillUI()
    {
        if (_skillModel == null) return;

        var slots = _skillModel.GetSkillSlots();
        for (int i = 0; i < skillSlotViews.Length; i++)
        {
            if (i < slots.Count && slots[i].skillData != null)
            {
                skillSlotViews[i].SetSkill(slots[i].skillData.Icon);
            }
            else
            {
                skillSlotViews[i].SetEmpty();
            }
        }
    }

    private void Unbind()
    {
        if (_stateModel != null)
            _stateModel.OnHpChanged -= OnHpChanged; 

        if (_skillModel != null)
            _skillModel.OnSkillChanged -= RefreshSkillUI;

        if (_levelModel != null)
        {
            _levelModel.OnExpChanged -= OnExpChanged;
            _levelModel.OnLevelUp -= OnLevelUp;
        }
    }

    private void OnDestroy()
    {
        Unbind();
    }
}
