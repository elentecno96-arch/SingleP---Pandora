using Game.Project.Scripts.Enemy.Interface;
using System.Collections.Generic;

namespace Game.Project.Scripts.Enemy
{
    /// <summary>
    /// 상태 저장소로 딕셔너리로 관리
    /// </summary>
    public class EnemyStateMachine
    {
        private Dictionary<EnemyStateType, IEnemyState> states = new();
        private IEnemyState currentState;

        public EnemyStateType CurrentStateType { get; private set; }

        public void Register(EnemyStateType type, IEnemyState state)
        {
            states[type] = state;
        }

        public void ChangeState(EnemyStateType newState)
        {
            currentState?.Exit();

            CurrentStateType = newState;
            currentState = states[newState];

            currentState.Enter();
        }

        public void Execute()
        {
            currentState?.Execute();
        }
    }
}
