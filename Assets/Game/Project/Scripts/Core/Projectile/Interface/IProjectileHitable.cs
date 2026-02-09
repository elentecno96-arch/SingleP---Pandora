using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.Interface
{
    public interface IProjectileHitable
    {
        /// <summary>
        /// 투사체가 충돌했을 때 실행될 로직
        /// </summary>
        bool OnHit(Projectile projectile, Collider other);
    }
}
