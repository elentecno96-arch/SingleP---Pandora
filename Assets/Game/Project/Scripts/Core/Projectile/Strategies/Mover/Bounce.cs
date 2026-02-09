using Game.Project.Scripts.Core.Projectile.Interface;
using System.Linq;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.Strategies.Mover
{
    /// <summary>
    /// ≈ı∞°√º∞° ∆®±Ë
    /// </summary>
    public class Bounce : IProjectileMover, IProjectileHitable
    {
        private const int DEFAULT_MAX_BOUNCE = 1;
        private const float TARGET_SEARCH_RADIUS = 10f;

        private ProjectileContext _ctx;
        private Vector3 _moveDirection;
        private int _currentBounceCount = 0;

        public void Init(ProjectileContext context, Projectile projectile)
        {
            _ctx = context;
            _moveDirection = context.direction.normalized;
            _currentBounceCount = 0;
        }

        public void OnUpdate(Projectile projectile)
        {
            projectile.transform.position += _moveDirection * _ctx.finalSpeed * Time.deltaTime;
        }

        public bool OnHit(Projectile projectile, Collider other)
        {
            if (_currentBounceCount < DEFAULT_MAX_BOUNCE)
            {
                _currentBounceCount++;
                Vector3 nextTargetDir = FindNextTargetDirection(projectile.transform.position, other);

                if (nextTargetDir != Vector3.zero)
                {
                    _moveDirection = nextTargetDir;
                }
                else
                {
                    Vector3 hitPoint = other.ClosestPoint(projectile.transform.position);
                    Vector3 normal = (projectile.transform.position - hitPoint).normalized;
                    if (normal == Vector3.zero) normal = -_moveDirection;

                    _moveDirection = Vector3.Reflect(_moveDirection, normal);
                }

                _moveDirection.y = 0;
                _moveDirection.Normalize();

                if (_moveDirection != Vector3.zero)
                    projectile.transform.rotation = Quaternion.LookRotation(_moveDirection);
                return false;
            }
            return true;
        }

        private Vector3 FindNextTargetDirection(Vector3 currentPos, Collider currentTarget)
        {
            Collider[] targets = Physics.OverlapSphere(currentPos, TARGET_SEARCH_RADIUS, LayerMask.GetMask("Enemy"));

            var nextTarget = targets
                .Where(t => t != currentTarget)
                .OrderBy(t => Vector3.Distance(currentPos, t.transform.position))
                .FirstOrDefault();

            if (nextTarget != null)
            {
                return (nextTarget.transform.position - currentPos).normalized;
            }

            return Vector3.zero;
        }
    }
}
