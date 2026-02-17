using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Project.Scripts.Managers.UI.In.ComBatUI.View
{
    /// <summary>
    /// 스킬 슬롯 UI
    /// </summary>
    public class SkillSlotUI : MonoBehaviour
    {
        [SerializeField] private Image skillIcon;
        [SerializeField] private Image cooldownOverlay;
        [SerializeField] private TextMeshProUGUI cooldownText;
        [SerializeField] private GameObject emptyVisual;

        // 스킬 아이콘 설정
        public void SetSkill(Sprite Icon)
        {
            if (Icon != null)
            {
                skillIcon.sprite = Icon;
                skillIcon.gameObject.SetActive(true);
                if (emptyVisual) emptyVisual.SetActive(false);
            }
            else
            {
                skillIcon.gameObject.SetActive(false);
            }
            UpdateCooldown(0, 1);
        }

        //쿨타임
        public void UpdateCooldown(float current, float max)
        {
            if (current <= 0 || max <= 0)
            {
                cooldownOverlay.fillAmount = 0;
                if (cooldownOverlay.gameObject.activeSelf) cooldownOverlay.gameObject.SetActive(false);
                cooldownText.text = "";
                return;
            }

            if (!cooldownOverlay.gameObject.activeSelf) cooldownOverlay.gameObject.SetActive(true);

            cooldownOverlay.fillAmount = current / max;
            cooldownText.text = current > 1.0f ? current.ToString("F0") : current.ToString("F1");
        }

        // 스킬 슬롯 비활성화
        public void SetEmpty()
        {
            skillIcon.gameObject.SetActive(false);
            if (emptyVisual) emptyVisual.SetActive(true);
            UpdateCooldown(0, 1);
        }
    }
}
