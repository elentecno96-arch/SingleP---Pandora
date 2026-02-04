using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Project.Scripts.Core.Projectile;

namespace Game.Project.Scripts.Core.Projectile.Interface
{
    /// <summary>
    /// 스킬의 속성이나 매커니즘을 변경 시키는 전략 정의 인터페이스
    /// </summary>
    public interface IProjectileStrategy
    {
        void Apply(ProjectileContext context);
    }
}
