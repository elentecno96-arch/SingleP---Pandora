using Game.Project.Scripts.Enemy.Interface;
using Game.Project.Scripts.Enemy.States;
using UnityEngine;

namespace Game.Project.Scripts.Enemy
{
    /// <summary>
    /// 일반 몬스터
    /// </summary>
    public class NormalEnemy : Enemy
    {
        [SerializeField] private Transform firePoint;
        public Transform FirePoint => firePoint;
        protected override void SetupStates()
        {
            stateMachine.Register(EnemyStateType.Spawn, new SpawnState(this));
            stateMachine.Register(EnemyStateType.Idle, new IdleState(this));
            stateMachine.Register(EnemyStateType.Move, new MoveState(this));
            stateMachine.Register(EnemyStateType.Attack, new AttackState(this));
            stateMachine.Register(EnemyStateType.Dead, new DeadState(this));
        }
    }
}
