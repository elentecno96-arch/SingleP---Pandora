using UnityEngine;
using UnityEngine.UI;

namespace Game.Project.Scripts.Enemy
{
    /// <summary>
    /// 적의 HP바
    /// </summary>
    public class EnemyHPBar : MonoBehaviour
    {
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private Canvas _canvas;

        private Transform _camTransform;
        private Enemy _owner;

        private void Awake()
        {
            if (Camera.main != null)
                _camTransform = Camera.main.transform;

            if (_canvas != null) _canvas.enabled = false;
        }

        /// <summary>
        /// 초기화
        /// </summary>
        public void Init(Enemy owner)
        {
            _owner = owner;

            if (_owner == null || _owner.Context == null) return;

            float maxHp = _owner.Context.currentStat.maxHp;

            if (maxHp <= 0) return;

            _hpSlider.maxValue = maxHp;
            _hpSlider.value = maxHp;

            if (_canvas != null) _canvas.enabled = false;
        }

        private void LateUpdate()
        {
            if (_canvas != null && _canvas.enabled && _camTransform != null)
            {
                transform.rotation = _camTransform.rotation;
            }
        }

        public void UpdateHP(float currentHp)
        {
            if (_owner == null || _hpSlider == null || _canvas == null) return;

            if (!_canvas.enabled && currentHp > 0)
                _canvas.enabled = true;

            float maxHp = _owner.Context.currentStat.maxHp;
            if (!Mathf.Approximately(_hpSlider.maxValue, maxHp))
            {
                _hpSlider.maxValue = maxHp;
            }

            _hpSlider.value = currentHp;

            if (currentHp <= 0)
            {
                _canvas.enabled = false;
            }
        }
    }
}
