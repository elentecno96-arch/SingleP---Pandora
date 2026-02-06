using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Interface;
using Game.Project.Scripts.Core.Projectile.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.Strategys.Mover
{
    /// <summary>
    /// 부채꼴 이동
    /// </summary>
    public class Spread : IProjectileMover
    {
        private Rigidbody _rb;

        public void Init(ProjectileContext context)
        {
            _rb = context.projectile.GetComponent<Rigidbody>();
            if (_rb != null)
            {
                _rb.useGravity = false;
                _rb.isKinematic = false;
                _rb.velocity = context.direction * context.data.speed;
            }
        }

        public void OnUpdate(Projectile projectile)
        {
            if (_rb != null && _rb.velocity.sqrMagnitude > 0.1f)
            {
                projectile.transform.rotation = Quaternion.LookRotation(_rb.velocity);
            }
        }
        public void OnFixedUpdate(Projectile projectile) { }
    }
}
