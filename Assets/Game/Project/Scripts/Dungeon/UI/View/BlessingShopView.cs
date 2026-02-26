using Game.Project.Data.Stat;
using Game.Project.Scripts.Dungeon.Blessing;
using Game.Project.Scripts.Managers.Singleton;
using TMPro;
using UnityEngine;

namespace Game.Project.Scripts.Managers.UI.Dungeon
{
    /// <summary>
    /// 축복 UI
    /// </summary>
    public class BlessingShopView : MonoBehaviour
    {
        [SerializeField] private BlessingBuyButton[] buySlots;

        [SerializeField] private TextMeshProUGUI currentGoldText;

        [SerializeField] private int baseStatPrice = 150;
        private bool _isGenerated = false;

        public void Show(bool state)
        {
            gameObject.SetActive(state);
            if (state) UpdateGoldDisplay();
        }

        public void RefreshShop()
        {
            GenerateOffers();
            _isGenerated = true;
        }

        /// <summary>
        /// 능력치 구매 슬롯 초기화
        /// </summary>
        private void GenerateOffers()
        {
            foreach (var slot in buySlots)
            {
                StatType randomType = (StatType)Random.Range(0, 7);
                float bonusValue = GetRandomBonusValue(randomType);
                int price = baseStatPrice + Random.Range(-20, 51);
                slot.Setup(randomType, bonusValue, price);
            }
        }

        /// <summary>
        /// 능력치 구매 시 처리
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="price"></param>
        /// <param name="slot"></param>
        public void TryPurchaseStat(StatType type, float value, int price, BlessingBuyButton slot)
        {
            if (PlayerManager.Instance.Inventory.Gold >= price)
            {
                PlayerManager.Instance.Inventory.AddGold(-price);

                UpdateGoldDisplay();

                Stat bonusStat = new Stat();
                ApplyValueToStatStruct(ref bonusStat, type, value);
                PlayerManager.Instance.Stats.AddAdditionalStat(bonusStat);

                slot.SetSoldOut();
            }
        }

        /// <summary>
        /// 능력치 구매 후 골드 새로고침
        /// </summary>
        private void UpdateGoldDisplay()
        {
            if (currentGoldText != null && PlayerManager.Instance != null)
            {
                currentGoldText.text = $"{PlayerManager.Instance.Inventory.Gold:N0} G";
            }
        }

        private float GetRandomBonusValue(StatType type)
        {
            return type switch
            {
                StatType.Damage => Random.Range(3, 8),
                StatType.MaxHP => Random.Range(30, 81),
                StatType.MoveSpeed => Random.Range(0.5f, 1.6f),
                StatType.Defense => Random.Range(1, 5),
                StatType.CastingSpeed => Random.Range(0.05f, 0.11f),
                StatType.CritChance => Random.Range(0.03f, 0.08f),
                StatType.CritDamage => Random.Range(0.1f, 0.26f),
                _ => 1f
            };
        }

        private void ApplyValueToStatStruct(ref Stat stat, StatType type, float value)
        {
            switch (type)
            {
                case StatType.MaxHP: stat.maxHp = value; break;
                case StatType.MoveSpeed: stat.maxMoveSpeed = value; break;
                case StatType.Defense: stat.defense = value; break;
                case StatType.Damage: stat.damage = value; break;
                case StatType.CastingSpeed: stat.castingSpeed = value; break;
                case StatType.CritDamage: stat.critDamage = value; break;
                case StatType.CritChance: stat.critChance = value; break;
            }
        }

        public void ResetShopForNextFloor()
        {
            _isGenerated = false;
        }
    }
}
