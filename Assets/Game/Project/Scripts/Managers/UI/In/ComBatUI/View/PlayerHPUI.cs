using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;

    public void SetHealth(float ratio)
    {
        if (hpSlider != null)
            hpSlider.value = ratio;
    }
}
