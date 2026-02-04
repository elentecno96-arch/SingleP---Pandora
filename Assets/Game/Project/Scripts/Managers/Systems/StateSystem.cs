using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems
{
    /// <summary>
    /// 플레이어의 생명 주기
    /// </summary>
    public class StateSystem : MonoBehaviour
    {
        public enum PlayerState { Alive, Dead, Invincible }

        [SerializeField] private float maxHp = 100f;
        private float _currentHp;

        public PlayerState CurrentState { get; private set; }
        public float HpRatio => _currentHp / maxHp;

        public event Action OnDead;
        public event Action<float> OnHpChanged;

        private bool _isInitialized = false;

        public void Init()
        {
            if (_isInitialized) return;

            _currentHp = maxHp;
            CurrentState = PlayerState.Alive;
            _isInitialized = true;
            Debug.Log("StateSystem: 초기화 완료");
        }

        public void TakeDamage(float damage)
        {
            if (CurrentState == PlayerState.Dead || CurrentState == PlayerState.Invincible) return;

            _currentHp = Mathf.Max(0, _currentHp - damage);
            OnHpChanged?.Invoke(_currentHp);

            if (_currentHp <= 0)
            {
                Die();
            }
        }
        private void Die()
        {
            CurrentState = PlayerState.Dead;
            OnDead?.Invoke();
            //플레이어 죽음 처리 예정
        }
    }
}
