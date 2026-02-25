using UnityEngine;
using Game.Project.Scripts.Dungeon.Manager;

 /// <summary>
 /// 다음 던전 층으로 이동시키는 트리거 (계단, 포탈 등)
 /// </summary>
 public class DungeonExitTrigger : MonoBehaviour
 {
     [SerializeField] private LayerMask _playerLayer;
     private bool _isTriggered = false;

     private void OnTriggerEnter(Collider other)
     {
         if (_isTriggered) return;
         if (((1 << other.gameObject.layer) & _playerLayer) == 0) return;

         _isTriggered = true;

         var stageManager = FindFirstObjectByType<DungeonStageManager>();
         if (stageManager != null)
         {
             Debug.Log("<color=yellow>다음 층으로 이동합니다.</color>");
             stageManager.ProceedToNextFloor();
         }
         gameObject.SetActive(false);
     }

     /// <summary>
     /// 새로운 층이 생성될 때 트리거 상태를 초기화
     /// </summary>
     public void ResetTrigger()
     {
         _isTriggered = false;
         gameObject.SetActive(true);
     }
 }