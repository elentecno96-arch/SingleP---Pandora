using Game.Project.Scripts.Core.Projectile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Data.Damage
{
    /// <summary>
    /// context에 담겨져 있는 정보를 이용하기 위한 대미지 인터페이스
    /// </summary>
    public interface IDamageable
    {
        void TakeDamage(ProjectileContext context);
    }
}
