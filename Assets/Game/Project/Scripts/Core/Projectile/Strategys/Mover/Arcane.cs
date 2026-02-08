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
    public class Arcane : IProjectileMover
    {
        private ProjectileContext _ctx;
        private Vector3 _moveDirection;

        private float spreadIntensity = 10f;

        public void Init(ProjectileContext context, Projectile projectile)
        {
            _ctx = context;

            projectile.transform.localScale = Vector3.one * _ctx.finalScale;

            float randomAngle = Random.Range(-spreadIntensity, spreadIntensity);

            Quaternion spreadRotation = Quaternion.Euler(0, randomAngle, 0);
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
