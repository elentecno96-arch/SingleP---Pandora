using Game.Project.Scripts.Core.Projectile.Interface;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.Strategies.Mover
{
    /// <summary>
    /// 직선 빠른 연사
    /// </summary>
    public class Rifle : IProjectileMover
    {
        private const float SPREAD_INTENSITY = 0.03f;

        private ProjectileContext _ctx;
        private Vector3 _moveDirection;

        public void Init(ProjectileContext context, Projectile projectile)
        {
            _ctx = context;
            projectile.transform.localScale = Vector3.one * _ctx.finalScale;

            Vector3 targetDir = _ctx.direction;

            Vector3 randomSpread = new Vector3(
                Random.Range(-SPREAD_INTENSITY, SPREAD_INTENSITY),
                Random.Range(-SPREAD_INTENSITY, SPREAD_INTENSITY),
                Random.Range(-SPREAD_INTENSITY, SPREAD_INTENSITY)
            );

            _moveDirection = (targetDir + randomSpread).normalized;

            if (_moveDirection != Vector3.zero)
                projectile.transform.rotation = Quaternion.LookRotation(_moveDirection);
        }

        public void OnUpdate(Projectile projectile)
        {
            projectile.transform.position += _moveDirection * _ctx.finalSpeed * Time.deltaTime;
        }
    }
}
