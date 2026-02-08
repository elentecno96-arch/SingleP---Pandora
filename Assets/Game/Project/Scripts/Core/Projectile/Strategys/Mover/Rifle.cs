using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.Strategys.Mover
{
    /// <summary>
    /// 직선 빠른 연사
    /// </summary>
    public class Rifle : IProjectileMover
    {
        private ProjectileContext _ctx;
        private Vector3 _moveDirection;

        public void Init(ProjectileContext context, Projectile projectile)
        {
            _ctx = context;
            projectile.transform.localScale = Vector3.one * _ctx.finalScale;

            float rifleSpread = 3.5f;
            float randomYaw = Random.Range(-rifleSpread, rifleSpread);
            float randomPitch = Random.Range(-rifleSpread * 0.5f, rifleSpread * 0.5f);

            Quaternion spreadRotation = Quaternion.Euler(randomPitch, randomYaw, 0);
            _moveDirection = spreadRotation * _ctx.direction;

            if (_moveDirection != Vector3.zero)
                projectile.transform.rotation = Quaternion.LookRotation(_moveDirection);
        }

        public void OnUpdate(Projectile projectile)
        {
            projectile.transform.position += _moveDirection * _ctx.finalSpeed * Time.deltaTime;
        }
    }
}
