using UnityEngine;
using Game.Project.Utility.Generic;
using Game.Project.Scripts.Managers.Systems.SpawnSystem;
using Game.Project.Scripts.Enemy.EnemySO;

namespace Game.Project.Scripts.Managers.Singleton
{
    /// <summary>
    /// 맵이나 적 스폰 담당 매니저
    /// </summary>
    public class SpawnManager : Singleton<SpawnManager>
    {
        [SerializeField] private EnemySpawnSystem _enemySpawnSystem;
        // [SerializeField] private MapSystem _mapSystem; // 추후 맵 관리 시스템

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

            _enemySpawnSystem.SpawnAt(data, position, multiplier);
        }
    }
}
