
namespace Game.Project.Scripts.Enemy.Interface
{
    /// <summary>
    /// 몬스터의 행동 규격
    /// </summary>
    public interface IEnemyState
    {
        void Enter();
        void Execute();
        void Exit();
    }
    /// <summary>
    /// 몬스터 상태 대칭 용
    /// </summary>
    public enum EnemyStateType
    {
        Spawn,
        Idle,
        Move,
        Attack,
        Dead
    }
}
