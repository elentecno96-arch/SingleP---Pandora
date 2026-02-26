using Game.Project.Data.Spawn;
using Game.Project.Scripts.Data.Items;
using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.UI.Dungeon;
using Game.Project.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FloorGrade { Normal, Unique, Legend }

[System.Serializable]
public class FloorGradeConfig
{
    public FloorGrade grade;
    public string gradeName;
    public Color gradeColor;
    public float baseMultiplier;
    public List<Game.Project.Scripts.Enemy.EnemySO.EnemyData> enemyList;
    [Range(0f, 100f)] public float weight;
}

namespace Game.Project.Scripts.Dungeon.Manager
{
    /// <summary>
    /// 던전 씬의 관리하는 매니저
    /// </summary>
    public class DungeonStageManager : MonoBehaviour
    {
        [SerializeField] private FadeManager fadeManager;
        [SerializeField] private DungeonInfoView infoUI;

        [SerializeField] private List<FloorGradeConfig> gradeConfigs;
        [SerializeField] private List<Transform> spawnPoints;

        [SerializeField] private GameObject lobbyGroup;
        [SerializeField] private Transform lobbySpawnPoint;

        [SerializeField] private GameObject dungeonGroup;
        [SerializeField] private GameObject exitStairsPrefab;

        [SerializeField] private List<SpawnTrigger> _allExitTriggers;
        [SerializeField] private ItemData startSkillBookItem;

        [SerializeField] private DungeonMediator dungeonMediator;

        private int _currentFloor = 0;
        private GameObject _activeExitStairs;

        public int CurrentFloor => _currentFloor;

        private void Awake()
        {
            if (GameManager.HasInstance)
            {
                GameManager.Instance.RegisterStageManager(this);
            }
        }

        public void ProceedToNextFloor()
        {
            StopAllCoroutines();
            StartCoroutine(TransitionRoutineCo());
        }

        private IEnumerator TransitionRoutineCo()
        {
            if (fadeManager != null)
                yield return StartCoroutine(fadeManager.FadeOutCo(2.5f));

            if (SpawnManager.HasInstance)
            {
                SpawnManager.Instance.ClearAllEnemies();
            }

            _currentFloor++;

            if (dungeonMediator != null)
            {
                dungeonMediator.InitializeBlessingForFloor();
            }

            if (_currentFloor == 1)
            {
                SaveDungeonEntryFlag();

                if (PlayerManager.HasInstance && startSkillBookItem != null)
                {
                    PlayerManager.Instance.Inventory.AddItem(startSkillBookItem, 1);
                    Debug.Log("<color=cyan>DungeonStageManager: 1층 진입 - 기초 마법서 지급 완료</color>");
                }
                PlayerManager.Instance.CurrentPlayer.GetComponent<PlayerCombat>().enabled = true;
            }

            FloorGradeConfig currentConfig = DetermineFloorGrade();
            float finalMultiplier = currentConfig.baseMultiplier + ((_currentFloor - 1) * 0.1f);

            if (lobbyGroup != null) lobbyGroup.SetActive(false);
            if (dungeonGroup != null) dungeonGroup.SetActive(true);

            SetupSpawnPositions();

            if (infoUI != null)
            {
                infoUI.UpdateInfo(_currentFloor, currentConfig, finalMultiplier);
            }

            foreach (var trigger in _allExitTriggers)
            {
                if (trigger == null) continue;
                trigger.SetSpawnData(currentConfig.enemyList, finalMultiplier);
                trigger.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(2f);

            if (infoUI != null) infoUI.gameObject.SetActive(true);

            if (fadeManager != null)
                yield return StartCoroutine(fadeManager.FadeInCo(1f));
        }

        private void SaveDungeonEntryFlag()
        {
            PlayerPrefs.SetInt("HasEnteredDungeon", 1);
            PlayerPrefs.Save();
            Debug.Log("<color=green>던전 진입 플래그 저장 완료</color>");
        }

        public void LoadAndStart()
        {
            Time.timeScale = 1f;

            if (infoUI != null)
            {
                infoUI.SetVisible(false);
            }

            if (PlayerManager.HasInstance)
            {
                PlayerManager.Instance.ResetForNewGame();

                PlayerManager.Instance.CurrentPlayer.GetComponent<PlayerCombat>().enabled = true;
            }

            if (lobbyGroup != null) lobbyGroup.SetActive(true);
            if (dungeonGroup != null) dungeonGroup.SetActive(false);

            _currentFloor = 0;

            if (lobbySpawnPoint != null)
            {
                MovePlayer(lobbySpawnPoint.position);
            }
        }

        public void ForceStartAtFloor(int floor)
        {
            _currentFloor = floor - 1;
            StopAllCoroutines();
            StartCoroutine(TransitionRoutineCo());
        }

        private void SetupSpawnPositions()
        {
            if (spawnPoints == null || spawnPoints.Count < 2) return;

            List<Transform> shuffleList = new List<Transform>(spawnPoints);
            for (int i = 0; i < shuffleList.Count; i++)
            {
                int rnd = Random.Range(i, shuffleList.Count);
                (shuffleList[i], shuffleList[rnd]) = (shuffleList[rnd], shuffleList[i]);
            }
            MovePlayer(shuffleList[0].position);

            if (_activeExitStairs != null) Destroy(_activeExitStairs);
            _activeExitStairs = Instantiate(exitStairsPrefab, shuffleList[1].position, Quaternion.identity);
        }

        private void MovePlayer(Vector3 pos)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                var controller = player.GetComponent<CharacterController>();
                if (controller != null) controller.enabled = false;

                player.transform.position = pos;

                if (controller != null) controller.enabled = true;
            }
        }

        private FloorGradeConfig DetermineFloorGrade()
        {
            float totalWeight = 0;
            foreach (var config in gradeConfigs) totalWeight += config.weight;

            float pivot = Random.Range(0f, totalWeight);
            float currentWeight = 0;

            foreach (var config in gradeConfigs)
            {
                currentWeight += config.weight;
                if (pivot <= currentWeight) return config;
            }
            return gradeConfigs[0];
        }
    }
}