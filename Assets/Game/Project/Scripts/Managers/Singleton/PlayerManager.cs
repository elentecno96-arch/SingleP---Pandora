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
        public InventorySystem Inventory { get; private set; }

        public PlayerCombat Combat { get; private set; }

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
            StatSource = new PlayerStatSource(Stats);

            if (Stats == null || State == null || skillEquip == null)
            {
                Debug.LogError("PlayerManager: 하위 시스템 누락");
                return;
            }

            Stats.Init();
            State.Init();
            skillEquip.init();
            Inventory.Init();

            _isInitialized = true;

            Debug.Log("PlayerManager: 시스템 초기화 완료");
        }

        public GameObject SpawnPlayer(Vector3 position)
        {
            if (!_isInitialized)
            {
                Debug.LogError("PlayerManager: Init 안됨");
                return null;
            }

            DestroyPlayer(); 

            var player = Instantiate(playerPrefab, position, Quaternion.identity);

            RegisterPlayer(player);

            player.GetComponent<Game.Project.Scripts.Player.Player>()?.Init();

            return player;
        }

        public void ResetForNewGame()
        {
            if (!_isInitialized) Init();

            Stats.ResetStats();
            Inventory.ClearInventory();
            skillEquip.ClearAllSlots();

            Debug.Log("플레이어 데이터 초기화 완료");
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

            Destroy(CurrentPlayer);
            UnregisterPlayer(CurrentPlayer);
        }
    }
}
