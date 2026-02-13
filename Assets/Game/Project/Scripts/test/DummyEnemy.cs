using Game.Project.Data.Damage;
using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Managers.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Test
{
    public class DummyEnemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHp = 100f;
        private float _currentHp;
        [SerializeField] private Color hitColor = Color.red;
        private Color _originColor;
        private MeshRenderer _renderer;
        private bool _isDead = false;

        private void Awake()
        {
            _renderer = GetComponentInChildren<MeshRenderer>();
            if (_renderer != null) _originColor = _renderer.material.color;
        }
        private void OnEnable()
        {
            _isDead = false;
            _currentHp = maxHp;
            if (_renderer != null) _renderer.material.color = _originColor;
        }

        public void TakeDamage(ProjectileContext context)
        {
            Debug.Log($"[Dummy] {gameObject.name} 대미지 받음: {context.finalDamage}, 크리티컬: {context.isCritical}");
            if (_isDead || !gameObject.activeInHierarchy) return;

            float damage = context.finalDamage;
            bool isCritical = context.isCritical;

            _currentHp -= damage;

            if (_currentHp <= 0)
            {
                _isDead = true;
                Die();
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(HitFeedback(isCritical));
            }
        }

        private void Die()
        {
            Debug.Log("[Dummy] 파괴됨!");
            gameObject.SetActive(false);
        }

        private IEnumerator HitFeedback(bool isCritical)
        {
            if (_renderer == null) yield break;

            _renderer.material.color = hitColor;
            yield return new WaitForSeconds(0.1f);
            _renderer.material.color = _originColor;
        }
    }
}
