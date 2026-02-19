using Game.Project.Data.Stat;
using TMPro;
using UnityEngine;

namespace Game.Project.Scripts.Managers.UI.StatInfo
{
    /// <summary>
    /// 플레이어 스탯 정보 UI 뷰
    /// </summary>
    public class StatDisplayView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _Lv;
        [SerializeField] private TextMeshProUGUI _hpText;
        [SerializeField] private TextMeshProUGUI _damageText;
        [SerializeField] private TextMeshProUGUI _defenseText;
        [SerializeField] private TextMeshProUGUI _moveSpeedText;
        [SerializeField] private TextMeshProUGUI _critChanceText;
        [SerializeField] private TextMeshProUGUI _critDamageText;

        //업데이트 스탯
        public void UpdateAllStats(int currentLevel, Stat stat)
        {
            Debug.Log($"[StatView] 데이터 수신 - HP: {stat.maxHp}, DMG: {stat.damage}");

            if (_Lv != null) _Lv.text = currentLevel.ToString();

            if (_hpText != null) _hpText.text = stat.maxHp.ToString();
            if (_damageText != null) _damageText.text = stat.damage.ToString();
            if (_defenseText != null) _defenseText.text = stat.defense.ToString();
            if (_moveSpeedText != null) _moveSpeedText.text = stat.maxMoveSpeed.ToString();
            if (_critChanceText != null) _critChanceText.text = $"{stat.critChance}%";
            if (_critDamageText != null) _critDamageText.text = $"{stat.critDamage}%";
        }
    }
}
