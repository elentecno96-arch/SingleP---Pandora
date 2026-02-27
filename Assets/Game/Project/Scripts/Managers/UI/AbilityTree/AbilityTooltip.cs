using Game.Project.Scripts.Core.Projectile.Rune;
using TMPro;
using UnityEngine;

namespace Game.Project.Scripts.Managers.UI.AbilityTree
{
    /// <summary>
    /// 스탯 트리 노드의 툴팁 
    /// </summary>
    public class AbilityTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descText;
        [SerializeField] private Vector2 _offset = new Vector2(0, 0); // 마우스와 툴팁 사이 간격

        private RectTransform _rectTransform;
        private Canvas _canvas;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            gameObject.SetActive(false);
        }

        private void Update()
        {
            FollowMouse();
        }

        private void FollowMouse()
        {
            Vector2 mousePos = Input.mousePosition;

            _rectTransform.position = mousePos + _offset;

            float rightEdge = Screen.width - (_rectTransform.rect.width * _canvas.scaleFactor);
            float topEdge = Screen.height - (_rectTransform.rect.height * _canvas.scaleFactor);

            Vector3 pos = _rectTransform.position;
            if (pos.x > rightEdge) pos.x = mousePos.x - _rectTransform.rect.width - _offset.x;
            if (pos.y > topEdge) pos.y = mousePos.y - _rectTransform.rect.height - _offset.y;

            _rectTransform.position = pos;
        }

        public void Show(AbilityNote data)
        {
            if (data == null) return;

            _nameText.text = data.nodeName;
            if (!string.IsNullOrEmpty(data.description))
            {
                _descText.text = data.description;
            }

            gameObject.SetActive(true);
            FollowMouse();
        }

        /// <summary>
        /// 룬 정보
        /// </summary>
        /// <param name="rune"></param>
        public void ShowRune(RuneData rune)
        {
            if (rune == null) return;

            _nameText.text = $"{rune.runeName}";

            string modifierName = GetModifierName(rune.modifier);
            string valueText = (rune.specialValue * 100f).ToString("F0");

            _descText.text = $"{modifierName} {valueText} 증가\n" +
                             $"{rune.description}";

            gameObject.SetActive(true);
            FollowMouse();
        }

        private string GetModifierName(ModifierType type)
        {
            return type switch
            {
                ModifierType.Damage => "공격력",
                ModifierType.CritChance => "치명타 확률",
                ModifierType.CritDamage => "치명타 피해",
                ModifierType.Cooldown => "재사용 대기시간 감소",
                _ => type.ToString()
            };
        }

        public void Hide() => gameObject.SetActive(false);
    }
}
