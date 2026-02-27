using Game.Project.Scripts.Enemy.EnemySO;
using Game.Project.Scripts.Managers.Singleton;
using System.Collections;
using System.Collections.Generic;
using Game.Project.Scripts.Dungeon.Manager;
using UnityEngine;

namespace Game.Project.Data.Spawn
{
    /// <summary>
    /// 플레이어 진입 시 적을 스폰하는 트리거
    /// </summary>
    public class SpawnTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private bool _isOneTime = true;
        [SerializeField] private bool _isExitTrigger = false;

        [SerializeField] private List<EnemyData> _spawnList;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private float _stageMultiplier = 1.0f;

        [SerializeField] private int _minSpawnCount = 3;
        [SerializeField] private int _maxSpawnCount = 5;
        [SerializeField] private float _spawnRadius = 3.0f;
        [SerializeField] private float _spawnInterval = 0.5f;

        private bool _hasTriggered = false;
        private Collider _triggerCollider;
        private int _currentSpawnBonus = 0;

        private void Awake()
        {
            _triggerCollider = GetComponent<Collider>();
            if (_triggerCollider != null)
            {
                _triggerCollider.isTrigger = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hasTriggered && _isOneTime) return;
            if (((1 << other.gameObject.layer) & _playerLayer) == 0) return;

            _hasTriggered = true;

            StartCoroutine(SpawnRoutineCo());

            if (_isExitTrigger)
            {
                var manager = FindFirstObjectByType<DungeonStageManager>();
                manager?.ProceedToNextFloor();
            }

            if (_isOneTime && _triggerCollider != null)
            {
                _triggerCollider.enabled = false;
            }
        }

        private IEnumerator SpawnRoutineCo()
        {
            if (_spawnList == null || _spawnList.Count == 0) yield break;

            int spawnCount = Random.Range(_minSpawnCount, _maxSpawnCount + 1) + _currentSpawnBonus;
            bool hasPoints = _spawnPoints != null && _spawnPoints.Length > 0;

            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 basePos = hasPoints
                    ? _spawnPoints[i % _spawnPoints.Length].position
                    : transform.position;

                Vector2 randomCircle = Random.insideUnitCircle * _spawnRadius;
                Vector3 finalPos = basePos + new Vector3(randomCircle.x, 0, randomCircle.y);

                var data = _spawnList[Random.Range(0, _spawnList.Count)];

                if (SpawnManager.Instance != null)
                {
                    SpawnManager.Instance.RequestEnemySpawn(data, finalPos, _stageMultiplier);
                }

                if (_spawnInterval > 0)
                    yield return new WaitForSeconds(_spawnInterval);
            }

            if (_isOneTime) gameObject.SetActive(false);
        }

        /// <summary>
        /// 던전 매니저가 매 층마다 새로운 데이터로 트리거를 초기화할 때 호출
        /// </summary>
        public void SetSpawnData(List<EnemyData> newList, float multiplier, int spawnBonus)
        {
            _spawnList = newList;
            _stageMultiplier = multiplier;
            _currentSpawnBonus = spawnBonus;
            _hasTriggered = false;

            if (_triggerCollider != null) _triggerCollider.enabled = true;
        }

        #region Debug (Gizmos)
        private void OnDrawGizmos()
        {
            BoxCollider box = GetComponent<BoxCollider>();
            if (box != null)
            {
                Gizmos.color = new Color(0, 1, 0, 0.2f);
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawCube(box.center, box.size);
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(box.center, box.size);
            }

            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.red;

            if (_spawnPoints != null && _spawnPoints.Length > 0)
            {
                foreach (var point in _spawnPoints)
                {
                    if (point == null) continue;
                    DrawCircleGizmo(point.position, _spawnRadius);
                }
            }
            else
            {
                DrawCircleGizmo(transform.position, _spawnRadius);
            }
        }

        private void DrawCircleGizmo(Vector3 center, float radius)
        {
            float angle = 0f;
            float step = (2f * Mathf.PI) / 20f; 
            Vector3 prevPos = center + new Vector3(radius, 0, 0);

            for (int i = 0; i <= 20; i++)
            {
                angle += step;
                Vector3 nextPos = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                Gizmos.DrawLine(prevPos, nextPos);
                prevPos = nextPos;
            }
        }
        #endregion
    }
}