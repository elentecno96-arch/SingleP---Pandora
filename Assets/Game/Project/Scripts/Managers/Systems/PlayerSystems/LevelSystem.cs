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
        private int currentLevel;
        private float currentExp;
        private int abilityPoints;

        private StatSystem statSystem;

        public float ExpRatio => (statSystem != null) ? currentExp / statSystem.GetMaxExp() : 0f;
        public int CurrentLevel => currentLevel;
        public int SkillPoint => abilityPoints;

        public event Action<int, float, float> OnExpChanged;
        public event Action<int> OnLevelUp;
        public event Action<int> OnPointChanged;

        public void Init()
        {
            statSystem = PlayerManager.Instance.Stats;

            currentLevel = 1;
            currentExp = 0;
            abilityPoints = 0;

            RefreshUI();
        }

        public void AddExp(float amount)
        {
            if (statSystem == null)
            {
                Debug.LogError("[LevelSystem] StatSystem이 null입니다!");
                return;
            }

            float maxLevel = statSystem.GetMaxLevel();
            if (currentLevel >= maxLevel) return;

            currentExp += amount;
            float maxExp = statSystem.GetMaxExp();


            while (currentExp >= maxExp)
            {
                if (currentLevel >= maxLevel)
                {
                    currentExp = 0;
                    break;
                }

                currentExp -= maxExp;
                LevelUp();

                maxExp = statSystem.GetMaxExp();
            }

            RefreshUI();
        }

        private void LevelUp()
        {
            currentLevel++;
            abilityPoints++;

            statSystem.ApplyLevelUp(currentLevel);

            var playerState = PlayerManager.Instance.State;
            if (playerState != null)
            {
                playerState.RecoverFullHP();
            }

            OnLevelUp?.Invoke(currentLevel);
            OnPointChanged?.Invoke(abilityPoints);
        }

        private void RefreshUI()
        {
            if (statSystem != null)
                OnExpChanged?.Invoke(currentLevel, currentExp, statSystem.GetMaxExp());
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
}
