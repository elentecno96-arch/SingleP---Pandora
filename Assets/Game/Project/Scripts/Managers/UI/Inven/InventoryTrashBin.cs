using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.UI.Inven;
using Game.Project.Scripts.Managers.UI.SkillBulid.View;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Project.Scripts.Managers.UI.Inven
{
    /// <summary>
    /// 인벤토리 아이템 제거 UI
    /// </summary>
    public class InventoryTrashBin : MonoBehaviour, IDropHandler
    {
        [SerializeField] private SkillBuildView ownerView;
        private const int SellPrice = 50;

        public void OnDrop(PointerEventData eventData)
        {
            InventorySlot fromSlot = eventData.pointerDrag?.GetComponent<InventorySlot>();

            if (fromSlot != null)
            {
                bool success = PlayerManager.Instance.Inventory.SellItem(fromSlot.slotIndex, SellPrice);

                if (success)
                {
                    if (ownerView != null)
                    {
                        ownerView.RefreshAll();
                        ownerView.HideItemTooltip();
                    }
                }
            }
        }
    }
}
