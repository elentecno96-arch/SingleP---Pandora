using Game.Project.Scripts.Data.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems.PlayerSystems
{
    /// <summary>
    /// 플레이어의 인벤토리 시스템
    /// </summary>
    public class InventorySystem : MonoBehaviour
    {
        private const int MAX_SLOT = 30;

        [SerializeField] private ItemSlot[] itemSlots = new ItemSlot[MAX_SLOT];

        public void Init()
        {
            if (itemSlots == null || itemSlots.Length != MAX_SLOT)
            {
                itemSlots = new ItemSlot[MAX_SLOT];
            }
            else
            {
                for (int i = 0; i < itemSlots.Length; i++)
                {
                    itemSlots[i] = null;
                }
            }
            Debug.Log("인벤토리 시스템 초기화 완료");
        }

        public bool AddItem(ItemData data, int amount = 1)
        {
            if (data == null) return false;

            if (data.maxStack > 1)
            {
                for (int i = 0; i < itemSlots.Length; i++)
                {
                    if (itemSlots[i] != null && itemSlots[i].itemData == data && itemSlots[i].count < data.maxStack)
                    {
                        itemSlots[i].count += amount;
                        return true;
                    }
                }
            }

            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i] == null || itemSlots[i].itemData == null)
                {
                    itemSlots[i] = new ItemSlot(data, amount);
                    return true;
                }
            }

            Debug.LogWarning("인벤토리가 가득 찼습니다!");
            return false;
        }

        public void SwapSlots(int startIdx, int endIdx)
        {
            if (startIdx < 0 || startIdx >= itemSlots.Length || endIdx < 0 || endIdx >= itemSlots.Length) return;

            ItemSlot startSlot = itemSlots[startIdx];
            ItemSlot endSlot = itemSlots[endIdx];

            if (startSlot == null) return;

            if (endSlot != null && endSlot.itemData != null && startSlot.itemData == endSlot.itemData)
            {
                int maxStack = startSlot.itemData.maxStack;
                if (endSlot.count < maxStack)
                {
                    int canAdd = maxStack - endSlot.count;
                    int moveAmount = Mathf.Min(canAdd, startSlot.count);

                    endSlot.count += moveAmount;
                    startSlot.count -= moveAmount;

                    if (startSlot.count <= 0)
                        itemSlots[startIdx] = null;

                    return;
                }
            }
            itemSlots[startIdx] = endSlot;
            itemSlots[endIdx] = startSlot;
        }

        public ItemSlot[] GetInventorySlots() => itemSlots;
    }
}
