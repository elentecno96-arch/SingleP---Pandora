using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.UI.Inven;
using Game.Project.Scripts.Player.Equip;  
using UnityEngine;
using System;
using TMPro;

namespace Game.Project.Scripts.Managers.UI.SkillBulid.View
{
    /// <summary>
    /// 스킬 빌드 패널 뷰
    /// </summary>
    public class SkillBuildView : MonoBehaviour
    {
        [SerializeField] private GameObject panelRoot; // 실질적인 UI 내용물이 담긴 오브젝트

        [SerializeField] private SkillBuildSlot[] skillSlotsUI;
        [SerializeField] private InventorySlot[] inventorySlotsUI;
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private ConfirmPopup equipConfirmPopup;
        [SerializeField] private SkillTooltipUI skillTooltip;
        [SerializeField] private ItemTooltipUI itemTooltip;

        public bool IsOpen => panelRoot != null && panelRoot.activeSelf;

        private void Start()
        {
            if (PlayerManager.Instance != null && PlayerManager.Instance.Inventory != null)
            {
                PlayerManager.Instance.Inventory.OnGoldChanged += UpdateGoldText;
            }
        }

        private void OnDestroy()
        {
            if (PlayerManager.Instance != null && PlayerManager.Instance.Inventory != null)
            {
                PlayerManager.Instance.Inventory.OnGoldChanged -= UpdateGoldText;
            }
        }

        //보여주기
        public void Show()
        {
            if (panelRoot == null) return;

            panelRoot.SetActive(true);

            if (PlayerManager.Instance?.Inventory != null)
            {
                UpdateGoldText(PlayerManager.Instance.Inventory.Gold);
            }

            RefreshAll();
        }

        public void Hide()
        {
            if (panelRoot != null) panelRoot.SetActive(false);
            HideTooltip();         
            HideItemTooltip();     
        }

        // 전체 새로고침
        public void RefreshAll()
        {
            RefreshSkills();
            RefreshInventory();

            if (PlayerManager.Instance != null && PlayerManager.Instance.Inventory != null)
            {
                UpdateGoldText(PlayerManager.Instance.Inventory.Gold);
            }
        }

        private void UpdateGoldText(int currentGold)
        {
            if (goldText != null)
            {
                goldText.text = currentGold.ToString("N0");
            }
        }

        // 스킬 슬롯 새로고침
        private void RefreshSkills()
        {
            var playerSlots = PlayerManager.Instance.skillEquip.GetSkillSlots();

            for (int i = 0; i < skillSlotsUI.Length; i++)
            {
                SkillSlot data = (playerSlots != null && i < playerSlots.Count) ? playerSlots[i] : null;
                skillSlotsUI[i].Refresh(data);
            }
        }

        // 인벤토리 새로고침
        private void RefreshInventory()
        {
            if (PlayerManager.Instance == null || PlayerManager.Instance.Inventory == null) return;
            var inventoryData = PlayerManager.Instance.Inventory.GetInventorySlots();

            for (int i = 0; i < inventorySlotsUI.Length; i++)
            {
                if (inventorySlotsUI[i] == null) continue;

                inventorySlotsUI[i].Init(this, i);

                if (i < inventoryData.Length && inventoryData[i] != null && inventoryData[i].itemData != null)
                {
                    inventorySlotsUI[i].SetItem(inventoryData[i].itemData, inventoryData[i].count);
                }
                else
                {
                    inventorySlotsUI[i].ClearSlot();
                }
            }
        }

        public void RequestEquip(Action onConfirm)
        {
            if (equipConfirmPopup != null)
            {
                equipConfirmPopup.Open(onConfirm);
            }
        }

        public void ShowTooltip(SkillSlot data)
        {
            if (skillTooltip != null) skillTooltip.Show(data);
        }

        public void HideTooltip()
        {
            if (skillTooltip != null) skillTooltip.Hide();
        }

        private void OnDisable()
        {
            HideTooltip();
        }

        public void ShowItemTooltip(string itemName)
        {
            if (itemTooltip != null) itemTooltip.Show(itemName);
        }

        public void HideItemTooltip()
        {
            if (itemTooltip != null) itemTooltip.Hide();
        }
    }
}
