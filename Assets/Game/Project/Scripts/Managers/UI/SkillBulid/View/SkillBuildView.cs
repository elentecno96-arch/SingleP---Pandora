using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.UI.Inven;
using UnityEngine;
using Game.Project.Scripts.Player.Equip;  

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
        public bool IsOpen => panelRoot != null && panelRoot.activeSelf;

        //보여주기
        public void Show()
        {
            if (panelRoot == null) return;

            panelRoot.SetActive(true);
            RefreshAll();
        }

        public void Hide()
        {
            if (panelRoot != null) panelRoot.SetActive(false);
        }

        // 전체 새로고침
        public void RefreshAll()
        {
            RefreshSkills();
            RefreshInventory();
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
    }
}
