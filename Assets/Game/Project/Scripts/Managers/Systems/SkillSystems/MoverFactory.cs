using Game.Project.Scripts.Core.Projectile.Interface;
using Game.Project.Scripts.Core.Projectile.SO;
using Game.Project.Scripts.Core.Projectile.Strategies.Mover;

namespace Game.Project.Scripts.Managers.Systems.SkillSystems
{
    /// <summary>
    /// 각 행동 체크
    /// </summary>
    public class MoverFactory
    {
        public IProjectileMover Create(MovementType type)
        {
            return type switch
            {
                MovementType.Linear => new Linear(),        //직선
                MovementType.Growing => new Growing(),      //곡사 포격
                MovementType.Rifle => new Rifle(),          //빠른 연사
                MovementType.Arcane => new Arcane(),        //확산 샷건
                MovementType.Spin => new Spin(),            //지속
                MovementType.Bounce => new Bounce(),        //도탄
                _ => new Linear()
            };
        }
    }
}
