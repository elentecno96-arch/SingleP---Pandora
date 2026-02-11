using Game.Project.Data.Stat;
using Game.Project.Scripts.Managers.Systems.PlayerSystems;

namespace Game.Project.Scripts.Player
{
    /// <summary>
    /// 플레이어 전용 스탯 전달자
    /// </summary>
    public class PlayerStatSource : IStatSourceable
    {
        private readonly StatSystem _statSystem;

        public PlayerStatSource(StatSystem statSystem)
        {
            _statSystem = statSystem;
        }
        
        /// <summary>
        /// 현재 스탯을 리턴하는 인터페이스 구현부
        /// </summary>
        /// <returns></returns>
        public Stat GetCurrentStat()
        {
            return _statSystem.CurrentStat;
        }
    }
}
