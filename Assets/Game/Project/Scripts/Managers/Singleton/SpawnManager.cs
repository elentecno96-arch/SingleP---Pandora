using Game.Project.Scripts.Enemy.EnemySO;
using Game.Project.Scripts.Managers.Systems.SpawnSystem;
using Game.Project.Utility.Generic;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Singleton
{
    /// <summary>
    /// 맵이나 적 스폰 담당 매니저
    /// </summary>
    public class SpawnManager : Singleton<SpawnManager>
    {
        [SerializeField] private EnemySpawnSystem _enemySpawnSystem;
        private List<GameObject> _activeEnemies = new List<GameObject>();

        private bool _isInitialized = false;

        protected override void Awake()
        {
            base.Awake();
            _enemySpawnSystem = GetComponentInChildren<EnemySpawnSystem>();
        }
        public void Init()
        {
            if (_isInitialized) return;

            Debug.Log("SpawnManager: 시스템 초기화 완료");
            _isInitialized = true;
        }

        /// <summary>
        /// 적 생성 요청
        /// </summary>
        /// <param name="data"></param>
        /// <param name="position"></param>
        /// <param name="multiplier"></param>
        public void RequestEnemySpawn(EnemyData data, Vector3 position, float multiplier)
        {
            if (_enemySpawnSystem == null)
                _enemySpawnSystem = GetComponentInChildren<EnemySpawnSystem>();

            GameObject spawnedEnemy = _enemySpawnSystem.SpawnAt(data, position, multiplier);

            if (spawnedEnemy != null)
            {
                _activeEnemies.Add(spawnedEnemy);
            }
        }

        /// <summary>
        /// 외의 적 제거 처리
        /// </summary>
        /// <param name="enemy"></param>
        public void RemoveActiveEnemy(GameObject enemy)
        {
            if (_activeEnemies.Contains(enemy))
            {
                _activeEnemies.Remove(enemy);
            }
        }

        /// <summary>
        /// 몬스터 제거 요청
        /// </summary>
        public void ClearAllEnemies()
        {
            for (int i = _activeEnemies.Count - 1; i >= 0; i--)
            {
                GameObject enemyObj = _activeEnemies[i];

                if (enemyObj != null)
                {
                    var enemyComponent = enemyObj.GetComponent<Game.Project.Scripts.Enemy.Enemy>();

                    if (enemyComponent != null)
                    {
                        PoolManager.Instance.ReturnEnemy(enemyComponent);
                    }
                    else
                    {
                        enemyObj.SetActive(false);
                    }
                }
            }
            _activeEnemies.Clear();
        }
    }
}
