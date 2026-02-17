using Game.Project.Scripts.Data.Items;

namespace Game.Project.Scripts.Managers.Systems.PlayerSystems
{
    /// <summary>
    /// 아이템 슬롯 정보를 담는 클래스
    /// </summary>
    [System.Serializable]
    public class ItemSlot
    {
        public ItemData itemData;
        public int count;

        public ItemSlot(ItemData data, int amount)
        {
            itemData = data;
            count = amount;
        }
    }
}
