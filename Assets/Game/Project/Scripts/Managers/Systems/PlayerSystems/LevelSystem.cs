using Game.Project.Scripts.Managers.Singleton;
using System;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems.PlayerSystems
{
    /// <summary>
    /// 플레이어 레벨 시스템
    /// 아직 밸런스 고려 안함
    /// </summary>
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
            }

            OnExpChanged?.Invoke(currentLevel, currentExp, maxExp);
        }

        private void LevelUp()
        {
            currentLevel++;
            abilityPoints++;

            //CheckSlotUnlock();

            OnLevelUp?.Invoke(currentLevel);
            OnPointChanged?.Invoke(abilityPoints);
        }

        //private void CheckSlotUnlock() { }

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
}
