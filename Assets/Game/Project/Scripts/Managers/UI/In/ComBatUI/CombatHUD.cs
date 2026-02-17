using Game.Project.Scripts.Managers.Systems.PlayerSystems;
using UnityEngine;
using Game.Project.Scripts.Managers.UI.In.ComBatUI.View;

public class CombatHUD : MonoBehaviour
{
    /// <summary>
    /// 인게임 화면에 플레이어 세부 정보 UI
    /// </summary>
    [SerializeField] private PlayerHPUI playerHPView;
    [SerializeField] private SkillSlotUI[] skillSlotViews;

    private StateSystem _stateModel;
    private SkillEquipSystem _skillModel;

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
        if (_stateModel != null) Unbind();

        _stateModel = model;
        _stateModel.OnHpChanged += UpdateView;

        UpdateView(0f);
    }

    private void UpdateView(float currentHp)
    {
        if (playerHPView != null && _stateModel != null)
        {
            playerHPView.SetHealth(_stateModel.HpRatio);
        }
    }

    public void BindSkills(SkillEquipSystem model)
    {
        if (_skillModel != null)
            _skillModel.OnSkillChanged -= RefreshSkillUI;

        _skillModel = model;
        _skillModel.OnSkillChanged += RefreshSkillUI;
        RefreshSkillUI();
    }

    private void RefreshSkillUI()
    {
        if (_skillModel == null) return;


        var slots = _skillModel.GetSkillSlots();
        for (int i = 0; i < skillSlotViews.Length; i++)
        {
            bool hasData = (i < slots.Count && slots[i].skillData != null);

            Debug.Log($"[UI 갱신] 인덱스:{i} / UI이름:{skillSlotViews[i].name} / 데이터존재:{hasData}");

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
            _stateModel.OnHpChanged -= UpdateView;
        if (_skillModel != null )
            _skillModel.OnSkillChanged -= RefreshSkillUI;
    }

    private void OnDestroy()
    {
        Unbind();
    }
}
