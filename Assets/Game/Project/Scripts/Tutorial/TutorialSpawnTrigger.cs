using Game.Project.Scripts.Enemy.EnemySO;
using Game.Project.Scripts.Managers.Singleton;
using UnityEngine;

namespace Game.Project.Scripts.Tutorial
{
    /// <summary>
    /// 튜토리얼 전용 적 소환 트리거
    /// </summary>
    public class TutorialSpawnTrigger : MonoBehaviour
    {
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private LayerMask _playerLayer;

        [TextArea]
        [SerializeField] private string _popupMessage = "적을 발견했습니다! 사거리 안에 들어가면 자동으로 공격합니다.";

        private bool _hasTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (_hasTriggered) return;

            if (((1 << other.gameObject.layer) & _playerLayer) != 0)
            {
                _hasTriggered = true;

                SpawnEnemy();

                if (UiManager.Instance != null)
                {
                    //UI를 통한 추가 팝업 예정
                    Debug.Log($"[Tutorial Message] : {_popupMessage}");
                }
                gameObject.SetActive(false);
            }
        }

        private void SpawnEnemy()
        {
            if (_enemyData == null || _spawnPoint == null)
            {
                Debug.LogWarning($"{gameObject.name}: EnemyData 또는 SpawnPoint가 누락되었습니다.");
                return;
            }
            SpawnManager.Instance.RequestEnemySpawn(_enemyData, _spawnPoint.position, 1.0f);
        }

        private void OnDrawGizmos()
        {
            // 에디터 뷰에서 소환 위치 시각화
            if (_spawnPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_spawnPoint.position, 0.5f);
                Gizmos.DrawLine(transform.position, _spawnPoint.position);
            }
        }
    }
}
