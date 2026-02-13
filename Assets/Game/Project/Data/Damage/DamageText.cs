using Game.Project.Scripts.Managers.Singleton;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.Project.Data.Damage
{
    /// <summary>
    /// 대미지 수치를 보여주는 컴포넌트
    /// </summary>
    public class DamageText : MonoBehaviour
    {
        private TextMeshProUGUI _textMesh;
        private Transform _camTransform;

        [SerializeField] private float _moveSpeed = 1.5f;   // 위로 떠오르는 속도
        [SerializeField] private float _alphaSpeed = 2f;    // 투명해지는 속도
        [SerializeField] private float _lifeTime = 1.0f;    // 화면에 머무는 시간

        private Color _color;
        private float _defaultFontSize;                     // 초기 폰트 크기 보관

        private void Awake()
        {
            _textMesh = GetComponentInChildren<TextMeshProUGUI>();

            if (_textMesh != null)
                _defaultFontSize = _textMesh.fontSize;

            if (Camera.main != null)
                _camTransform = Camera.main.transform;
        }

        /// <summary>
        /// 대미지 텍스트의 초기 상태를 설정하고 연출
        /// </summary>
        /// <param name="damage">표시할 대미지 수치</param>
        /// <param name="isCritical">크리티컬 여부 (노란색 + 크기 증가)</param>
        public void Setup(float damage, bool isCritical)
        {
            if (_textMesh == null) return;

            _textMesh.text = Mathf.RoundToInt(damage).ToString();
            _textMesh.fontSize = isCritical ? _defaultFontSize * 1.4f : _defaultFontSize;

            _color = isCritical ? Color.yellow : Color.white;
            _color.a = 1f; 
            _textMesh.color = _color;

            gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(ReturnDelay(_lifeTime));
        }

        /// <summary>
        /// 지정 시간 후 풀링 반납
        /// </summary>
        private IEnumerator ReturnDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (PoolManager.Instance != null)
            {
                PoolManager.Instance.ReturnDamageText(this);
            }
        }

        private void LateUpdate()
        {
            transform.Translate(Vector3.up * _moveSpeed * Time.deltaTime, Space.World);

            if (_camTransform != null)
                transform.rotation = _camTransform.rotation;

            if (_color.a > 0)
            {
                _color.a -= _alphaSpeed * Time.deltaTime;
                _textMesh.color = _color;
            }
        }
    }
}