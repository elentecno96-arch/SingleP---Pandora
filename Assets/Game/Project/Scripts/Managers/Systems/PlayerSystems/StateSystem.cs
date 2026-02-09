using System;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems.PlayerSystems
{
    /// <summary>
    /// 플레이어의 생명 주기
    /// </summary>
    public class StateSystem : MonoBehaviour
    {
        public enum PlayerState { Alive, Dead, Invincible }

        private float _currentHp;
        private StatSystem _statSystem;

        public PlayerState CurrentState { get; private set; }
        public float HpRatio => _currentHp / _statSystem.CurrentStat.maxHp;

        public event Action OnDead;
        public event Action<float> OnHpChanged;

        private bool _isInitialized = false;

        public void Init()
        {
            if (_isInitialized) return;

            _statSystem = GetComponent<StatSystem>();
            _currentHp = _statSystem.CurrentStat.maxHp;

            CurrentState = PlayerState.Alive;
            _isInitialized = true;
            Debug.Log("StateSystem: 초기화 완료");
        }

        public void TakeDamage(float damage)
        {
            if (CurrentState == PlayerState.Dead || CurrentState == PlayerState.Invincible) return;
            float finalDamage = Mathf.Max(1, damage - _statSystem.CurrentStat.defense);

            _currentHp = Mathf.Max(0, _currentHp - finalDamage);
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
