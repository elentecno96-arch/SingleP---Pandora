using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Interface;
using Game.Project.Scripts.Core.Projectile.States;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.Strategys.Mover
{
    public class Spin : IProjectileMover, IProjectileHitable
    {
        private ProjectileContext _ctx;
        private bool _isAnchored = false;
        private float _anchorTimer = 0f;
        private float _maxAnchorDuration = 2.0f;
        private float _hitInterval = 0.2f;
        private float _lastHitTime = 0f;

        private Vector3 _startPos;
        private float _currentHorizontalDist = 0f;
        private Transform _targetTransform;
        private Vector3 _attachOffset;

        private const float MAX_HEIGHT = 2.5f;

        public void Init(ProjectileContext context, Projectile projectile)
        {
            _ctx = context;
            _isAnchored = false;
            _anchorTimer = 0f;
            _currentHorizontalDist = 0f;
            _targetTransform = null;

            _startPos = projectile.transform.position;

            Vector3 horizontalDir = new Vector3(_ctx.direction.x, 0, _ctx.direction.z).normalized;
            _ctx.direction = horizontalDir;

            projectile.transform.rotation = Quaternion.LookRotation(_ctx.direction);
        }

        public void OnUpdate(Projectile projectile)
        {
            projectile.transform.Rotate(Vector3.up, 1200f * Time.deltaTime, Space.World);

            if (!_isAnchored)
            {
                float moveStep = _ctx.finalSpeed * Time.deltaTime;
                _currentHorizontalDist += moveStep;

                float progress = Mathf.Clamp01(_currentHorizontalDist / _ctx.finalRange);
                Vector3 nextPos = _startPos + (_ctx.direction * _currentHorizontalDist);

                float heightOffset = Mathf.Sin(progress * Mathf.PI) * MAX_HEIGHT;

                nextPos.y = Mathf.Lerp(_startPos.y, 1.5f, progress) + heightOffset;

                projectile.transform.position = nextPos;
                if (progress >= 1.0f)
                {
                    projectile.ChangeState(ProjectileStates.Impact);
                }
            }
            else
            {
                if (_targetTransform != null && _targetTransform.gameObject.activeInHierarchy)
                {
                    projectile.transform.position = _targetTransform.position + _attachOffset;
                }

                _anchorTimer += Time.deltaTime;
                if (Time.time >= _lastHitTime + _hitInterval)
                {
                    ApplyAreaDamage(projectile);
                    _lastHitTime = Time.time;
                }

                if (_anchorTimer >= _maxAnchorDuration)
                {
                    projectile.ChangeState(ProjectileStates.Impact);
                }
            }
        }

        public bool OnHit(Projectile projectile, Collider other)
        {
            if (_isAnchored) return false;
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                return true;
            }
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                AnchorToTarget(projectile, other);
                return false;
            }

            return false;
        }

        private void AnchorToTarget(Projectile projectile, Collider other)
        {
            _isAnchored = true;
            _lastHitTime = Time.time;
            _targetTransform = other.transform;
            _attachOffset = new Vector3(0, 1.0f, 0);
            projectile.transform.position = _targetTransform.position + _attachOffset;
        }

        private void ApplyAreaDamage(Projectile projectile)
        {
            float radius = 2.0f * _ctx.finalScale;
            Collider[] targets = Physics.OverlapSphere(projectile.transform.position, radius, LayerMask.GetMask("Enemy"));
            foreach (var target in targets)
            {
                projectile.ImpactStateCall(target.gameObject);
            }
        }
    }
}

