using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using Game.Project.Scripts.Data.Items;
using Game.Project.Scripts.Player.Equip;

namespace Game.Project.Scripts.Managers.UI.SkillBulid.View
{
    /// <summary>
    /// 스킬 툴팁 UI
    /// </summary>
    public class SkillTooltipUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI skillNameText;
        [SerializeField] private TextMeshProUGUI skillDescText;
        [SerializeField] private TextMeshProUGUI skillStatText;

        [SerializeField] private GameObject runeSection;
        [SerializeField] private TextMeshProUGUI runeListText;

        [SerializeField] private Vector2 offset = new Vector2(-20f, -20f);

        private RectTransform _rectTransform;
        private Canvas _canvas;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
        }

        private void Update()
        {
            FollowMouse();
        }

        private void FollowMouse()
        {
            Vector2 mousePos = Input.mousePosition;

            _rectTransform.position = mousePos + offset;

            float canvasScale = _canvas.scaleFactor;
            float width = _rectTransform.rect.width * canvasScale;
            float height = _rectTransform.rect.height * canvasScale;

            Vector3 pos = _rectTransform.position;

            if (pos.x - width < 0)
                pos.x = mousePos.x + Mathf.Abs(offset.x) + width;

            if (pos.y - height < 0)
                pos.y = mousePos.y + Mathf.Abs(offset.y) + height;

            _rectTransform.position = pos;
        }

        public void Show(SkillSlot slot)
        {
            if (slot == null || slot.IsEmpty) return;

            skillNameText.text = slot.skillData.skillName;
            skillDescText.text = slot.skillData.description;

            var ctx = slot.context;
            StringBuilder statSb = new StringBuilder();
            statSb.AppendLine($"데미지: {ctx.finalDamage}");
            statSb.AppendLine($"쿨타임: {ctx.finalCooldown}s");
            statSb.AppendLine($"치명타: {ctx.finalCritChance * 100}%");
            skillStatText.text = statSb.ToString();

            if (slot.equippedRunes != null && slot.equippedRunes.Count > 0)
            {
                runeSection.SetActive(true);
                StringBuilder runeSb = new StringBuilder();
                foreach (var rune in slot.equippedRunes)
                {
                    runeSb.AppendLine($"<color=#FFD700>• {rune.itemName}</color>");
                    runeSb.AppendLine($"{rune.description}\n");
                }
                runeListText.text = runeSb.ToString();
            }
            else
            {
                runeSection.SetActive(false);
            }

            gameObject.SetActive(true);
            FollowMouse();
        }

        public void Hide() => gameObject.SetActive(false);
    }
}
