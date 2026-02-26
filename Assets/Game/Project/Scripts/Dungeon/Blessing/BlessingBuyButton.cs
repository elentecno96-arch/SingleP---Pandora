using Game.Project.Scripts.Dungeon.Blessing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Project.Scripts.Managers.UI.Dungeon
{
    /// <summary>
    /// 축복 UI 버튼
    /// </summary>
    public class BlessingBuyButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Button buyBtn;

        private StatType _type;
        private float _value;
        private int _price;

        public void Setup(StatType type, float value, int price)
        {
            _type = type;
            _value = value;
            _price = price;
            buyBtn.interactable = true;

            string valDisplay = (type == StatType.CritChance || type == StatType.CastingSpeed || type == StatType.CritDamage)
                ? $"+{(int)(value * 100)}%" : $"+{(int)value}";

            infoText.text = $"{GetStatName(type)}\n{valDisplay}";
            priceText.text = $"{_price} G";
        }

        public void OnClickBuy()
        {
            GetComponentInParent<BlessingShopView>()
                .TryPurchaseStat(_type, _value, _price, this);
        }

        public void SetSoldOut()
        {
            buyBtn.interactable = false;
            infoText.text = "<color=green>축복 완료</color>";
            priceText.text = "-";
        }

        private string GetStatName(StatType type)
        {
            return type switch
            {
                StatType.MaxHP => "최대 체력",
                StatType.MoveSpeed => "이동 속도",
                StatType.Defense => "방어력",
                StatType.Damage => "공격력",
                StatType.CastingSpeed => "시전 속도",
                StatType.CritDamage => "치명타 위력",
                StatType.CritChance => "치명타 확률",
                _ => "능력치"
            };
        }
    }
}
