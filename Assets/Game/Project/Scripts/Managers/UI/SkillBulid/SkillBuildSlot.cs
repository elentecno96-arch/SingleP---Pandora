using Game.Project.Scripts.Data.Items;
using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.UI.Inven;
using Game.Project.Scripts.Managers.UI.SkillBulid.View;
using Game.Project.Scripts.Managers.Systems.PlayerSystems;  
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
    public class SkillBuildSlot : MonoBehaviour, IPointerClickHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image skillIcon;   
        [SerializeField] private GameObject selectionVisual;
        [SerializeField] private GameObject lockVisual;

        [SerializeField] private GameObject runeParent;
        [SerializeField] private List<Image> runeIcons;

        [SerializeField] private int slotIndex; // 스킬 슬롯의 번호 (0~5)
        private SkillSlot _currentData;

        private readonly Color _emptyRuneColor = new Color(1, 1, 1, 0.1f);

        public void Refresh(SkillSlot data)
        {
            _currentData = data;

            if (data == null || data.IsEmpty)
            {
                skillIcon.gameObject.SetActive(false);
                runeParent.SetActive(false);
                return;
            }

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
            var skillEquipSystem = PlayerManager.Instance.skillEquip;
            var currentSlot = skillEquipSystem.GetSkillSlots()[slotIndex];

            if (itemData.type == ItemType.SkillBook && !currentSlot.IsEmpty)
            {
                var view = GetComponentInParent<SkillBuildView>();
                if (view != null)
                {
                    view.RequestEquip(() => PerformEquip(inventory, itemData));
                }
            }
            else
            {
                PerformEquip(inventory, itemData);
            }
        }

        private void PerformEquip(InventorySystem inventory, ItemData itemData)
        {
            bool success = false;

            switch (itemData.type)
            {
                case ItemType.SkillBook:
                    success = PlayerManager.Instance.skillEquip.EquipSkill(slotIndex, itemData.skillData);
                    break;
                case ItemType.Rune:
                    success = PlayerManager.Instance.skillEquip.EquipRune(slotIndex, itemData);
                    break;
            }

            if (success)
            {
                inventory.RemoveItem(itemData, 1);

                var view = GetComponentInParent<SkillBuildView>();
                if (view != null) view.RefreshAll();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SetSelected(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_currentData == null || _currentData.IsEmpty) return;

            var view = GetComponentInParent<SkillBuildView>();
            if (view != null)
            {
                view.ShowTooltip(_currentData);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            var view = GetComponentInParent<SkillBuildView>();
            if (view != null)
            {
                view.HideTooltip();
            }
        }

        public void SetSelected(bool isSelected)
        {
            if (selectionVisual != null)
                selectionVisual.SetActive(isSelected);
        }
    }
}
