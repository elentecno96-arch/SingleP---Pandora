using Game.Project.Scripts.Enemy.Interface;
using UnityEngine;

namespace Game.Project.Scripts.Enemy.States
{
    /// <summary>
    /// 적의 공격 상태
    /// </summary>
    public class AttackState : IEnemyState
    {
        private readonly Enemy _owner;
        private float _timer;
        private bool _isFired;

        private float _attackDuration;           // 공격 동작 총 시간
        private float _fireTiming;               // 실제 공격이 발생하는 시점
        private float _rotationLockTime;         // 타겟 추적을 멈추는 시점

        public AttackState(Enemy owner) => _owner = owner;

        public void Enter()
        {
            _timer = 0f;
            _isFired = false;

            _owner.StopMoving();

            float speedMod = _owner.Context.currentStat.castingSpeed;
            if (speedMod <= 0.01f) speedMod = 1f;

            float skillCooldown = _owner.Data.skillData.cooldown;

            _attackDuration = skillCooldown / speedMod;
            _fireTiming = _attackDuration * 0.5f;

            _rotationLockTime = _fireTiming * 0.3f;

            _owner.EnemyCombat.ShowWarning(_fireTiming, _owner.Data.attackRange);

            _owner.SetAttackAnim();
        }

        public void Execute()
        {
            if (_owner.Context.currentStat.maxHp <= 0 || _owner.Context.target == null) return;

            _timer += Time.deltaTime;

            if (!_isFired && _timer < _rotationLockTime)
            {
                _owner.LookAtTarget(_owner.Context.target.transform.position);
            }

            if (!_isFired && _timer >= _fireTiming)
            {
                _isFired = true;
                _owner.ActiveAttack();
            }

            if (_timer >= _attackDuration)
            {
                CheckNext();
            }
        }

        /// <summary>
        /// 공격 완료 후 재 공격
        /// </summary>
        private void CheckNext()
        {
            float distance = _owner.GetTargetDistance();

            if (distance <= _owner.Data.attackRange * 1.1f)
                _owner.ChangeState(EnemyStateType.Attack);
            else
                _owner.ChangeState(EnemyStateType.Move);
        }

        public void Exit() { }
    }
}
