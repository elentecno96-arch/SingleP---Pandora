using Game.Project.Scripts.Data.Items;
using Game.Project.Scripts.Managers.Singleton;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems.PlayerSystems
{
    /// <summary>
    /// 플레이어의 인벤토리 시스템
    /// </summary>
    public class InventorySystem : MonoBehaviour
    {
        private const int MAX_SLOT = 30;

        [SerializeField] private AudioClip goldAcquisitionSfx;

        [SerializeField] private ItemSlot[] itemSlots = new ItemSlot[MAX_SLOT];

        public int Gold { get; private set; }
        public event Action OnInventoryChanged;
        public event Action<int> OnGoldChanged;

        public void Init()
        {
            // 배열 크기 보장 및 초기화
            if (itemSlots == null || itemSlots.Length != MAX_SLOT)
            {
                itemSlots = new ItemSlot[MAX_SLOT];
            }

            ClearInventory();
            Debug.Log("인벤토리 시스템 초기화 완료");
        }

        public bool AddItem(ItemData data, int amount = 1)
        {
            if (data == null || amount <= 0) return false;

            // 골드 처리
            if (data.type == ItemType.Gold)
            {
                Gold += (data.goldcAmount * amount);
                OnGoldChanged?.Invoke(Gold);

                UiManager.Instance.ShowAcquisitionPopup(data, amount);

                return true;
            }

            int remainingAmount = amount;

            // 중첩 가능한지 확인
            if (data.maxStack > 1)
            {
                for (int i = 0; i < itemSlots.Length; i++)
                {
                    if (itemSlots[i] != null && itemSlots[i].itemData == data)
                    {
                        int canAdd = data.maxStack - itemSlots[i].count;
                        int addAmount = Mathf.Min(canAdd, remainingAmount);

                        itemSlots[i].count += addAmount;
                        remainingAmount -= addAmount;

                        if (remainingAmount <= 0) break;
                    }
                }
            }

            // 남은 아이템을 빈 슬롯에 추가
            while (remainingAmount > 0)
            {
                int emptySlotIndex = Array.FindIndex(itemSlots, slot => slot == null || slot.itemData == null);

                if (emptySlotIndex == -1)
                {
                    Debug.LogWarning("인벤토리가 가득 찼습니다.");
                    OnInventoryChanged?.Invoke();
                    return false;
                }

                int addAmount = Mathf.Min(data.maxStack, remainingAmount);
                itemSlots[emptySlotIndex] = new ItemSlot(data, addAmount);
                remainingAmount -= addAmount;
            }

            OnInventoryChanged?.Invoke();

            UiManager.Instance.ShowAcquisitionPopup(data, amount);

            return true;
        }

        /// 슬롯 간 아이템 이동 및 교체
        public void SwapSlots(int startIdx, int endIdx)
        {
            if (startIdx < 0 || startIdx >= itemSlots.Length || endIdx < 0 || endIdx >= itemSlots.Length) return;
            if (startIdx == endIdx) return;

            ItemSlot startSlot = itemSlots[startIdx];
            ItemSlot endSlot = itemSlots[endIdx];

            if (startSlot == null || startSlot.itemData == null) return;

            if (endSlot != null && endSlot.itemData == startSlot.itemData && startSlot.itemData.maxStack > 1)
            {
                int maxStack = startSlot.itemData.maxStack;
                int canAdd = maxStack - endSlot.count;
                int moveAmount = Mathf.Min(canAdd, startSlot.count);

                endSlot.count += moveAmount;
                startSlot.count -= moveAmount;

                if (startSlot.count <= 0)
                    itemSlots[startIdx] = null;
            }
            else
            {
                itemSlots[startIdx] = endSlot;
                itemSlots[endIdx] = startSlot;
            }

            OnInventoryChanged?.Invoke();
        }

        /// <summary>
        /// 아이템 사용
        /// </summary>
        public bool RemoveItem(ItemData data, int amount = 1)
        {
            int totalCount = itemSlots.Where(s => s != null && s.itemData == data).Sum(s => s.count);
            if (totalCount < amount) return false;

            int toRemove = amount;
            for (int i = itemSlots.Length - 1; i >= 0; i--) 
            {
                if (itemSlots[i] != null && itemSlots[i].itemData == data)
                {
                    if (itemSlots[i].count > toRemove)
                    {
                        itemSlots[i].count -= toRemove;
                        toRemove = 0;
                        break;
                    }
                    else
                    {
                        toRemove -= itemSlots[i].count;
                        itemSlots[i] = null;
                    }
                }
                if (toRemove <= 0) break;
            }

            OnInventoryChanged?.Invoke();
            return true;
        }

        public void AddGold(int amount)
        {
            if (amount == 0) return;

            Gold += amount;

            if (Gold < 0) Gold = 0;

            if (amount > 0 && AudioManager.HasInstance)
                AudioManager.Instance.PlaySfx(goldAcquisitionSfx);

            OnGoldChanged?.Invoke(Gold);

            if (GameManager.HasInstance)
            {
                GameManager.Instance.AddGainedGold(amount);
            }
            Debug.Log($"[Inventory] 골드 변동: {amount}, 현재 잔액: {Gold}");
        }

        public void ClearInventory()
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                itemSlots[i] = null;
            }
            Gold = 0;

            OnInventoryChanged?.Invoke();
            OnGoldChanged?.Invoke(Gold);
        }

        public ItemSlot[] GetInventorySlots() => itemSlots;
    }
}
