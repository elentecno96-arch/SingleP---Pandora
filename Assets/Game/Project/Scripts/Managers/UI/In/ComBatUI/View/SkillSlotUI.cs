using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private GameObject emptyVisual;

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

    public void UpdateCooldown(float current, float max)
    {
        if (current <= 0)
        {
            cooldownOverlay.fillAmount = 0;
            cooldownOverlay.gameObject.SetActive(false);
            cooldownText.text = "";
            return;
        }

        cooldownOverlay.gameObject.SetActive(true);
        cooldownOverlay.fillAmount = current / max;
        cooldownText.text = current > 1.0f ? current.ToString("F0") : current.ToString("F1");
    }

    public void SetEmpty()
    {
        skillIcon.gameObject.SetActive(false);
        if (emptyVisual) emptyVisual.SetActive(true);
    }
}
