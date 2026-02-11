
namespace Game.Project.Data.Stat
{
    /// <summary>
    /// 스탯의 정보를 전달하기 위한 인터페이스
    /// </summary>
    public interface IStatSourceable
    {
        Stat GetCurrentStat();
    }
}
