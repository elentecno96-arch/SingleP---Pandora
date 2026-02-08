using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Project.Scripts.Core.Projectile;

namespace Game.Project.Scripts.Core.Projectile.Interface
{
    /// <summary>
    /// 투사체의 상태를 정의하는 인터페이스
    /// </summary>
    public interface IProjectileState
    {
        void Enter(ProjectileContext context, Projectile projectile);
        void UpdateState(Projectile projectile);
        void Exit(ProjectileContext context);
    }
}
