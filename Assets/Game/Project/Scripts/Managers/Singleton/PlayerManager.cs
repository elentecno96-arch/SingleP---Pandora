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
        public SkillEquipSystem skillEquip { get; private set; }
        public InventorySystem Inventory { get; private set; }
        public LevelSystem levelSystem { get; private set; }
        public AbilitySystem abilitySystem { get; private set; }

        public PlayerStatSource StatSource { get; private set; }

        [SerializeField] private GameObject playerPrefab;
        public GameObject CurrentPlayer { get; private set; }

        private bool _isInitialized = false;

        public void Init()
        {
            if (_isInitialized) return;

            Stats = GetComponentInChildren<StatSystem>(true);
            State = GetComponentInChildren<StateSystem>(true);
            skillEquip = GetComponentInChildren<SkillEquipSystem>(true);
            Inventory = GetComponentInChildren<InventorySystem>(true);
            levelSystem = GetComponentInChildren<LevelSystem>(true);
            abilitySystem = GetComponentInChildren<AbilitySystem>(true);

            if (Stats == null || State == null || skillEquip == null)
            {
                Debug.LogError("PlayerManager: 필수 하위 시스템이 누락되었습니다.");
                return;
            }

            StatSource = new PlayerStatSource(Stats);

            Stats.Init();
            State.Init();
            levelSystem.Init();
            abilitySystem.Init();
            skillEquip.init();
            Inventory.Init();

            _isInitialized = true;
            Debug.Log("PlayerManager: 모든 하위 시스템 초기화 완료");
        }

        /// <summary>
        /// 모든 데이터를 초기값(1레벨, 빈 가방 등)으로 리셋
        /// </summary>
        public void ResetForNewGame()
        {
            if (!_isInitialized) Init();

            if (levelSystem != null) levelSystem.Init();
            if (Stats != null) Stats.ResetStats();

            if (State != null)
            {
                State.RecoverFullHP(); 
                State.SetAlive();      
            }

            if (Inventory != null) Inventory.ClearInventory();
            if (skillEquip != null) skillEquip.ClearAllSlots();
            if (abilitySystem != null) abilitySystem.Init();

        }

        public GameObject SpawnPlayer(Vector3 position)
        {
            if (!_isInitialized) Init();

            DestroyPlayer();

            var player = Instantiate(playerPrefab, position, Quaternion.identity);
            RegisterPlayer(player);

            player.GetComponent<Game.Project.Scripts.Player.Player>()?.Init();

            return player;
        }

        private void RegisterPlayer(GameObject player)
        {
            CurrentPlayer = player;
            Combat = player.GetComponent<PlayerCombat>();
            Combat?.Init(this);
        }

        public void UnregisterPlayer(GameObject player)
        {
            if (CurrentPlayer != player) return;
            CurrentPlayer = null;
            Combat = null;
        }

        public void DestroyPlayer()
        {
            if (CurrentPlayer == null) return;

            var target = CurrentPlayer;
            UnregisterPlayer(target);
            Destroy(target);
        }
        public PlayerCombat Combat { get; private set; }
    }
}
