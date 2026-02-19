using Game.Project.Data.Stat;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatDisplayView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _Lv;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private TextMeshProUGUI _defenseText;
    [SerializeField] private TextMeshProUGUI _moveSpeedText;
    [SerializeField] private TextMeshProUGUI _critChanceText;
    [SerializeField] private TextMeshProUGUI _critDamageText;

    public void UpdateAllStats(Stat stat)
    {
        Debug.Log($"[StatView] 데이터 수신 - HP: {stat.maxHp}, DMG: {stat.damage}");

        _Lv.text = stat.maxLevel.ToString(); 
        _hpText.text = stat.maxHp.ToString();
        _damageText.text = stat.damage.ToString();
        _defenseText.text = stat.defense.ToString();
        _moveSpeedText.text = stat.maxMoveSpeed.ToString();
        _critChanceText.text = $"{stat.critChance}%";
        _critDamageText.text = $"{stat.critDamage}%";
    }
}
