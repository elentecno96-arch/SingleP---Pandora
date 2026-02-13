using Game.Project.Scripts.Enemy.EnemySO;
using Game.Project.Scripts.Managers.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Data.Spawn
{
    /// <summary>
    /// 스폰 트리거
    /// </summary>
    public class SpawnTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask _playerLayer;

        [SerializeField] private List<EnemyData> _spawnList;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private float _stageMultiplier = 1.0f;
        [SerializeField] private bool _isOneTime = true;

        [SerializeField] private int _minSpawnCount = 3;
        [SerializeField] private int _maxSpawnCount = 5;

        [SerializeField] private float _spawnRadius = 3.0f;
        [SerializeField] private float _spawnInterval = 0.5f;

        private bool _hasTriggered = false;
        private void Awake()
        {
            if (TryGetComponent(out BoxCollider box))
            {
                box.isTrigger = true;
            }
        }
        private void OnTriggerEnter(Collider foreign)
        {
            if (_hasTriggered && _isOneTime) return;
            if (((1 << foreign.gameObject.layer) & _playerLayer) != 0)
            {
                _hasTriggered = true;

                StartCoroutine(SpawnRoutineCo());

                if (_isOneTime)
                {
                    if (TryGetComponent(out Collider c)) c.enabled = false;
                }
            }
        }

        /// <summary>
        /// 점진적으로 적을 소환하는 루틴
        /// </summary>
        private IEnumerator SpawnRoutineCo()
        {
            if (_spawnPoints == null || _spawnPoints.Length == 0) yield break;
            if (_spawnList == null || _spawnList.Count == 0) yield break;

            int spawnCount = Random.Range(_minSpawnCount, _maxSpawnCount + 1);

            for (int i = 0; i < spawnCount; i++)
            {
                Transform basePoint = _spawnPoints[i % _spawnPoints.Length];
                Vector2 randomCircle = Random.insideUnitCircle * _spawnRadius;
                Vector3 spawnOffset = new Vector3(randomCircle.x, 0, randomCircle.y);
                Vector3 finalPos = basePoint.position + spawnOffset;

                var data = _spawnList[Random.Range(0, _spawnList.Count)];

                if (SpawnManager.Instance != null)
                {
                    SpawnManager.Instance.RequestEnemySpawn(data, finalPos, _stageMultiplier);
                }

                if (_spawnInterval > 0)
                    yield return new WaitForSeconds(_spawnInterval);
                else
                    yield return null;
            }

            if (_isOneTime) gameObject.SetActive(false);
        }

        /// <summary>
        /// 시각화
        /// </summary>
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

            if (_spawnPoints == null) return;

            Gizmos.matrix = Matrix4x4.identity;
            foreach (var point in _spawnPoints)
            {
                if (point != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(point.position, 0.3f);
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(transform.position, point.position);
                }
            }
        }
    }
}
