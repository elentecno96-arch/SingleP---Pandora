using Game.Project.Scripts.Managers.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Project.Scripts.Managers.UI.In.ComBatUI.View
{
    public class PlayerExpUI : MonoBehaviour
    {
        /// <summary>
        /// 경험치 UI, 레벨 텍스트
        /// </summary>
        [SerializeField] private Slider expSlider;
        [SerializeField] private TextMeshProUGUI levelText;

        public void SetExp(float ratio, int level)
        {
            if (expSlider != null)
            {

                expSlider.value = ratio;
            }

            if (levelText != null)
            {
                levelText.text = $"{level}";
            }
        }
    }
}
