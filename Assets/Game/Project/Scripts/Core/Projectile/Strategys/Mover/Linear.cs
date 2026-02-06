using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.Strategys.Mover
{
    /// <summary>
    /// 직선 이동 방식
    /// </summary>
    public class Linear : IProjectileMover
    {
        private Rigidbody _rb;
        private float _boostMultiplier = 2.5f;

        public void Init(ProjectileContext context)
        {
            _rb = context.projectile.GetComponent<Rigidbody>();
            if (_rb != null)
            {
                _rb.useGravity = false;
                _rb.isKinematic = false;

                Vector3 initialVelocity = context.direction * (context.data.speed * _boostMultiplier);
                _rb.velocity = initialVelocity;
            }
        }

        public void OnUpdate(Projectile projectile)
        {
            if (_rb == null) return;

            float currentSpeed = _rb.velocity.magnitude;
            float targetSpeed = projectile.Context.data.speed;

            if (currentSpeed > targetSpeed)
            {
                _rb.velocity = Vector3.Lerp(_rb.velocity, _rb.velocity.normalized * targetSpeed, Time.deltaTime * 5f);
            }
            if (_rb.velocity.sqrMagnitude > 0.1f)
            {
                projectile.transform.rotation = Quaternion.LookRotation(_rb.velocity);
            }
        }

        public void OnFixedUpdate(Projectile projectile) { }
    }
}
