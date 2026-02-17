using Game.Project.Scripts.Data.Items;
using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.UI.Inven;
using Game.Project.Scripts.Managers.UI.SkillBulid.View;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Game.Project.Scripts.Player.Equip;

namespace Game.Project.Scripts.Managers.UI.SkillBulid
{
    /// <summary>
    /// 스킬 슬롯의 UI
    /// </summary>
    public class SkillBuildSlot : MonoBehaviour, IPointerClickHandler, IDropHandler
    {
        [SerializeField] private Image skillIcon;   
        [SerializeField] private GameObject selectionVisual;
        [SerializeField] private GameObject lockVisual;

        [SerializeField] private GameObject runeParent;
        [SerializeField] private List<Image> runeIcons;

        [SerializeField] private int slotIndex; // 스킬 슬롯의 번호 (0~5)

        private readonly Color _emptyRuneColor = new Color(1, 1, 1, 0.1f);

        public void Refresh(SkillSlot data)
        {
            if (data == null || data.IsEmpty)
            {
                skillIcon.gameObject.SetActive(false);
                runeParent.SetActive(false);
                return;
            }

            skillIcon.gameObject.SetActive(true);
            skillIcon.sprite = data.skillData.Icon;

            int maxRuneCount = data.GetMaxRuneCount();
            runeParent.SetActive(maxRuneCount > 0);

            for (int i = 0; i < runeIcons.Count; i++)
            {
                GameObject runeSlotObj = runeIcons[i].transform.parent.gameObject;
                bool isAvailable = i < maxRuneCount;
                runeSlotObj.SetActive(isAvailable);

                if (isAvailable)
                {
                    bool isEquipped = i < data.equippedRunes.Count;
                    runeIcons[i].sprite = isEquipped ? data.equippedRunes[i].icon : null;
                    runeIcons[i].color = isEquipped ? Color.white : _emptyRuneColor;
                }
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            InventorySlot fromSlot = eventData.pointerDrag?.GetComponent<InventorySlot>();

            if (fromSlot != null)
            {
                ExecuteEquip(fromSlot.slotIndex);
            }
        }

        private void ExecuteEquip(int inventoryIndex)
        {
            var inventory = PlayerManager.Instance.Inventory;
            var itemSlot = inventory.GetInventorySlots()[inventoryIndex];

            if (itemSlot == null || itemSlot.itemData == null) return;

            var itemData = itemSlot.itemData;
            bool success = false;

            Debug.Log($"<color=cyan>[Equip]</color> {itemData.itemName} 장착 시도 중...");

            switch (itemData.type)
            {
                case ItemType.SkillBook:
                    // 스킬 장착: 해당 스킬북의 skillData를 현재 슬롯(slotIndex)에 장착
                    success = PlayerManager.Instance.skillEquip.EquipSkill(slotIndex, itemData.skillData);
                    break;

                case ItemType.Rune:
                    // 룬 장착: 해당 룬을 현재 스킬 슬롯(slotIndex)에 장착
                    success = PlayerManager.Instance.skillEquip.EquipRune(slotIndex, itemData);
                    break;

                default:
                    Debug.Log("<color=red>결과:</color> 장착 불가능한 아이템입니다.");
                    break;
            }

            if (success)
            {
                // 장착 성공 시 UI 전체 새로고침
                var view = GetComponentInParent<SkillBuildView>();
                if (view != null) view.RefreshAll();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // 다른 모든 슬롯의 선택 해제는 보통 View나 Manager에서 처리하는 것이 좋지만, 
            // 여기서는 단순하게 본인 선택만 표시합니다.
            SetSelected(true);
        }

        public void SetSelected(bool isSelected)
        {
            if (selectionVisual != null)
                selectionVisual.SetActive(isSelected);
        }
    }
}
