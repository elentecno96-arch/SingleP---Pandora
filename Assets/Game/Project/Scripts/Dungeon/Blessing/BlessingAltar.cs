using Game.Project.Scripts.Managers.UI.Dungeon;
using UnityEngine;

namespace Game.Project.Scripts.Dungeon.Blessing
{
    /// <summary>
    /// 축복 트리거
    /// </summary>
    public class BlessingTrigger : MonoBehaviour
    {
        [SerializeField] private DungeonMediator mediator;
        private bool _isUsedInThisFloor = false; 

        private void OnTriggerEnter(Collider other)
        {
            if (_isUsedInThisFloor || !other.CompareTag("Player")) return;

            _isUsedInThisFloor = true;
            mediator.OpenBlessingShop();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                mediator.CloseBlessingShop();
            }
        }
    }
}
