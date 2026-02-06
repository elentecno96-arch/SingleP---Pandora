using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.Interface
{
    public interface IProjectileMover
    {
        void Init(ProjectileContext context);
        void OnUpdate(Projectile projectile);
        void OnFixedUpdate(Projectile projectile);
    }
}
