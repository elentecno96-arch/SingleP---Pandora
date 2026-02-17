using UnityEngine;
using UnityEngine.UI;

namespace Game.Project.Scripts.Managers.UI.In.ComBatUI.View
{
    /// <summary>
    /// 플레이어 Hp UI
    /// </summary>
    public class PlayerHPUI : MonoBehaviour
    {
        [SerializeField] private Slider hpSlider;

        public void SetHealth(float ratio)
        {
            if (hpSlider != null)
                hpSlider.value = ratio;
        }
    }
}
