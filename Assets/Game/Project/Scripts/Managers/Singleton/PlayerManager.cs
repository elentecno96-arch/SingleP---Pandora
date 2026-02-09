using Game.Project.Scripts.Managers.Systems.PlayerSystems;
using Game.Project.Scripts.Player;
using Game.Project.Utility.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Singleton
{
    /// <summary>
    /// 플레이어의 상태 담당하는 매니저
    /// </summary>
    public class PlayerManager : Singleton<PlayerManager>
    {
        public StatSystem Stats { get; private set; }
        public StateSystem State { get; private set; }
        public SkillEquipSystem skillEquip {  get; private set; }

        public PlayerCombat Combat { get; private set; }

        [SerializeField] private GameObject playerObject;

        private bool _isInitialized = false;

        public void Init()
        {
            if (_isInitialized) return;
            Stats = GetComponentInChildren<StatSystem>();
            State = GetComponentInChildren<StateSystem>();
            skillEquip = GetComponentInChildren<SkillEquipSystem>();

            if (playerObject != null)
            {
                Combat = playerObject.GetComponent<PlayerCombat>();
            }

            if (skillEquip) skillEquip.init();
            if (Stats) Stats.Init();
            if (State) State.Init();

            if (Combat != null)
            {
                Combat.Init(this);
            }
               
            _isInitialized = true;
            Debug.Log("PlayerManager: 초기화 완료");
        }
    }
}
