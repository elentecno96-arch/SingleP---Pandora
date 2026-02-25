using Game.Project.Data.Damage;
using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Managers.Singleton;
using System;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems.PlayerSystems
{
    /// <summary>
    /// 플레이어의 생명 주기
    /// </summary>
    public class StateSystem : MonoBehaviour,IDamageable
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

        public void SetAlive()
        {
            CurrentState = PlayerState.Alive;
            _isInitialized = true;
        }

        public void TakeDamage(ProjectileContext context)
        {
            if (CurrentState == PlayerState.Dead || CurrentState == PlayerState.Invincible) return;

            float damage = context.finalDamage;

            // 크리티컬 계산
            if (context.isCritical)
            {
                damage *= context.finalCritDamage;
            }

            //2중 크리티컬 가능성 제거( 주석 처리)
            //float finalDamage = Mathf.Max(1, damage - _statSystem.CurrentStat.defense);
            float finalDamage = Mathf.Max(1, context.finalDamage - _statSystem.CurrentStat.defense);

            _currentHp = Mathf.Max(0, _currentHp - finalDamage);
            OnHpChanged?.Invoke(_currentHp);

            if (_currentHp <= 0) Die();

            Debug.Log($"플레이어 피격! 원본:{damage} -> 최종:{finalDamage} / 남은 체력:{_currentHp}");
        }

        /// <summary>
        /// 레벨업 또는 그외 방법으로 플레이어 회복
        /// </summary>
        public void RecoverFullHP()
        {
            if (_statSystem == null) return;

            _currentHp = _statSystem.CurrentStat.maxHp;

            OnHpChanged?.Invoke(_currentHp);
        }

        public void SetHP(float hp)
        {
            _currentHp = Mathf.Clamp(hp, 0, _statSystem.CurrentStat.maxHp);

            OnHpChanged?.Invoke(_currentHp);

            if (_currentHp <= 0) Die();
            else CurrentState = PlayerState.Alive;
        }

        private void Die()
        {
            if (CurrentState == PlayerState.Dead) return; // 중복 실행 방지

            CurrentState = PlayerState.Dead;
            OnDead?.Invoke();

            Debug.Log("<color=red>플레이어 사망!</color>");

            if (GameManager.HasInstance)
            {
                GameManager.Instance.PlayerDeath();
            }
        }
    }
}
