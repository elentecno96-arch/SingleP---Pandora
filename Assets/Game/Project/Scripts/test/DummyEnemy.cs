using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Project.Data.Damage;

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

        public void TakeDamage(float amount)
        {
            if (_isDead || !gameObject.activeInHierarchy) return;

            _currentHp -= amount;
            Debug.Log($"[Dummy] µ•πÃ¡ˆ ¿‘¿Ω: {amount} | ≥≤¿∫ HP: {maxHp}");

            if (_currentHp <= 0)
            {
                _isDead = true;
                Die();
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(HitFeedback());
            }
        }

        private void Die()
        {
            Debug.Log("[Dummy] ∆ƒ±´µ !");
            gameObject.SetActive(false);
        }

        private IEnumerator HitFeedback()
        {
            if (_renderer == null) yield break;

            _renderer.material.color = hitColor;
            yield return new WaitForSeconds(0.1f);
            _renderer.material.color = _originColor;
        }
    }
}
