using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.Systems.PlayerSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    int currentLevel;
    float currentExp;
    int abilityPoints;

    StatSystem statSystem;

    public event Action<int, float, float> OnExpChanged; 
    public event Action<int> OnLevelUp;
    public event Action<int> OnPointChanged;

    public int CurrentLevel => currentLevel;
    public int SkillPoint => abilityPoints;

    public void Init()
    {
        statSystem = PlayerManager.Instance.Stats;

        currentLevel = 1;
        currentExp = 0;
        abilityPoints = 10;
    }

    public void AddExp(float amount)
    {
        if (currentLevel >= statSystem.GetMaxLevel()) return;

        currentExp += amount;

        float maxExp = statSystem.GetMaxExp();
        while (currentExp >= maxExp)
        {
            LevelUp();
            currentExp -= maxExp;
            // 레벨업에 따른 다음 필요 경험치 증가 로직이 필요하다면 여기서 조정
        }

        OnExpChanged?.Invoke(currentLevel, currentExp, maxExp);
    }

    private void LevelUp()
    {
        currentLevel++;
        abilityPoints++;

        // 스킬 슬롯 해금 체크 (기존 계획: 5렙, 10렙)
        CheckSlotUnlock();

        OnLevelUp?.Invoke(currentLevel);
        OnPointChanged?.Invoke(abilityPoints);
        Debug.Log($"<color=yellow>LEVEL UP! 현재 레벨: {currentLevel}</color>");
    }

    private void CheckSlotUnlock()
    {
        // PlayerManager 등을 통해 SkillEquipSystem에 해금 신호 전달
    }

    public bool UsePoint(int amount = 1)
    {
        if (abilityPoints < amount) return false;
        abilityPoints -= amount;
        OnPointChanged?.Invoke(abilityPoints);
        return true;
    }

    public void AddSkillPoint(int amount)
    {
        abilityPoints += amount;
        OnPointChanged?.Invoke(abilityPoints);
    }
}
