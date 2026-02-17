using Game.Project.Scripts.Data.Items;
using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.UI.SkillBulid.View;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Project.Scripts.Managers.UI.Inven
{
    /// <summary>
    /// 인벤토리 슬롯 UI
    /// </summary>
    public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI countText;

        private Transform _originalParent;

        public int slotIndex;
        private ItemData _currentItem;
        private int _currentCount;

        public void SetItem(ItemData data, int count)
        {
            _currentItem = data;
            _currentCount = count;

            if (data == null || count <= 0)
            {
                ClearSlot();
                return;
            }

            itemIcon.sprite = data.icon;
            itemIcon.gameObject.SetActive(true);

            countText.text = count > 1 ? count.ToString() : "";
            countText.gameObject.SetActive(count > 1);
        }

        public void ClearSlot()
        {
            _currentItem = null;
            _currentCount = 0;
            itemIcon.gameObject.SetActive(false);
            countText.gameObject.SetActive(false);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_currentItem == null) return;

            _originalParent = itemIcon.transform.parent;

            Canvas mainCanvas = GetComponentInParent<Canvas>();
            if (mainCanvas != null)
            {
                itemIcon.transform.SetParent(mainCanvas.transform);
            }

            itemIcon.transform.SetAsLastSibling(); 
            itemIcon.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (itemIcon.gameObject.activeSelf == false) return;
            itemIcon.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            itemIcon.raycastTarget = true;
            itemIcon.transform.SetParent(_originalParent);
            itemIcon.transform.localPosition = Vector3.zero;
        }

        public void OnDrop(PointerEventData eventData)
        {
            InventorySlot fromSlot = eventData.pointerDrag?.GetComponent<InventorySlot>();

            if (fromSlot != null && fromSlot != this)
            {
                PlayerManager.Instance.Inventory.SwapSlots(fromSlot.slotIndex, this.slotIndex);

                var view = GetComponentInParent<SkillBuildView>();
                if (view != null) view.RefreshAll();
            }
        }
    }
}
