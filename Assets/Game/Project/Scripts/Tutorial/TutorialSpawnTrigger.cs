using Game.Project.Scripts.Enemy.EnemySO;
using Game.Project.Scripts.Managers.Singleton;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Game.Project.Scripts.Tutorial
{
    /// <summary>
    /// 튜토리얼 전용 적 소환 트리거
    /// </summary>
    public class TutorialSpawnTrigger : MonoBehaviour
    {
        [System.Serializable]
        public struct SpawnInfo
        {
            public List<EnemyData> enemy;
            public Transform centerPoint; 
            public float spawnRadius;     
            public float delayBeforeSpawn;
        }

        [SerializeField] private List<SpawnInfo> _spawns = new List<SpawnInfo>();

        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private string _popupMessage = "주변에서 적들이 나타납니다!";

        private bool _hasTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (_hasTriggered) return;

            if (((1 << other.gameObject.layer) & _playerLayer) != 0)
            {
                _hasTriggered = true;
                StartCoroutine(Co_SequentialSpawn());
                Debug.Log($"[Tutorial] {_popupMessage}");
            }
        }

        private IEnumerator Co_SequentialSpawn()
        {
            foreach (var info in _spawns)
            {
                if (info.enemy == null || info.enemy.Count == 0 || info.centerPoint == null) continue;

                if (info.delayBeforeSpawn > 0f)
                {
                    yield return new WaitForSeconds(info.delayBeforeSpawn);
                }

                foreach (var enemyData in info.enemy)
                {
                    if (enemyData == null) continue;

                    Vector2 randomCircle = Random.insideUnitCircle * info.spawnRadius;
                    Vector3 randomPosition = info.centerPoint.position + new Vector3(randomCircle.x, 0, randomCircle.y);

                    SpawnManager.Instance.RequestEnemySpawn(enemyData, randomPosition, 0f);
                }
            }
            gameObject.SetActive(false); 
        }

        private void OnDrawGizmos()
        {
            if (_spawns == null) return;

            foreach (var info in _spawns)
            {
                if (info.centerPoint != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(info.centerPoint.position, 0.3f);

                    Gizmos.color = new Color(0, 1, 1, 0.3f);
                    Gizmos.DrawWireSphere(info.centerPoint.position, info.spawnRadius);

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(transform.position, info.centerPoint.position);
                }
            }
        }
    }
}
