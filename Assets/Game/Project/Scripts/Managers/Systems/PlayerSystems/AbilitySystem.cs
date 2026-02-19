using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.UI.AbilityTree;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems.PlayerSystems
{
    /// <summary>
    /// 플레이어의 스탯 강화 시스템
    /// </summary>
    public class AbilitySystem : MonoBehaviour
    {
        [SerializeField] private List<AbilityNote> _abilityNodes = new List<AbilityNote>();
        public List<AbilityNote> AbilityNodes => _abilityNodes;

        private LevelSystem _levelSystem;
        private StatSystem _statSystem;

        public void Init()
        {
            if (PlayerManager.Instance == null) return;

            _levelSystem = PlayerManager.Instance.levelSystem;
            _statSystem = PlayerManager.Instance.Stats;

            if (_abilityNodes == null) return;

            foreach (var node in _abilityNodes)
            {
                if (node != null)
                {
                    node.isUnlocked = false;
                }
            }
        }

        /// <summary>
        /// 특정 노드 해금
        /// </summary>
        public bool TryUnlockNode(AbilityNote node)
        {
            if (node.isUnlocked) return false; //해금 여부
            if (_levelSystem.SkillPoint <= 0) return false; //포인트 여부
            if (_levelSystem.CurrentLevel < node.requiredLevel) return false; //레벨 제한 여부
            if (node.abilityNode != null && !node.abilityNode.isUnlocked) return false; //앞에 노드 해금 여부

            if (_levelSystem.UsePoint(1))
            {
                node.isUnlocked = true;
                _statSystem.AddAdditionalStat(node.bonusStat);
                return true;
            }

            return false;
        }

        public void ResetAllAbilities()
        {
            int recoveredPoints = 0;

            foreach (var node in _abilityNodes)
            {
                if (node != null && node.isUnlocked)
                {
                    node.isUnlocked = false;
                    recoveredPoints++;
                }
            }

            if (recoveredPoints > 0)
            {
                _statSystem.ResetAdditionalStat();

                _levelSystem.AddSkillPoint(recoveredPoints);
            }
        }
    }
}
