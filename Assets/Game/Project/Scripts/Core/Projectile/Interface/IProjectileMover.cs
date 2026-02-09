
namespace Game.Project.Scripts.Core.Projectile.Interface
{
    public interface IProjectileMover
    {
        void Init(ProjectileContext context, Projectile projectile);
        void OnUpdate(Projectile projectile);
    }
}
