using Game.Project.Scripts.Core.Projectile.Interface;
using System.Linq;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.Strategies.Mover
{
    /// <summary>
    /// 포물선 크기 증가 스킬
    /// </summary>
    public class Growing : IProjectileMover
    {
        private const float MOVE_DURATION = 1.2f;
        private const float START_SCALE_MULTIPLIER = 0.2f;
        private const float LOOK_AHEAD_STEP = 0.01f;
        private const float TARGET_SEARCH_RADIUS = 10f;
        private const float ROTATION_THRESHOLD = 0.99f;

        private ProjectileContext _ctx;
        private Vector3 _startPos;
        private Vector3 _targetPos;

        private float _duration;
        private float _elapsedTime;
        private float _arcHeight = 6f;

        public void Init(ProjectileContext context, Projectile projectile)
        {
            _ctx = context;
            _startPos = projectile.transform.position;

            _targetPos = FindTarget(projectile.transform.position, projectile);

            if (_targetPos == Vector3.zero)
            {
                _targetPos = _startPos + (_ctx.direction * _ctx.finalRange);
            }
            _duration = MOVE_DURATION;
            _elapsedTime = 0f;

            projectile.transform.localScale = Vector3.one * (_ctx.finalScale * START_SCALE_MULTIPLIER);

            var childParticles = projectile.GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in childParticles)
            {
                var main = ps.main;
                main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            }
        }

        public void OnUpdate(Projectile projectile)
        {
            _elapsedTime += Time.deltaTime;
            float t = _elapsedTime / _duration;

            if (t <= 1.0f)
            {
                Vector3 currentPos = Vector3.Lerp(_startPos, _targetPos, t);
                currentPos.y += Mathf.Sin(t * Mathf.PI) * _arcHeight;

                projectile.transform.position = currentPos;

                float currentScale = Mathf.Lerp(_ctx.finalScale * START_SCALE_MULTIPLIER, _ctx.finalScale, t);
                projectile.transform.localScale = Vector3.one * currentScale;

                if (t < ROTATION_THRESHOLD)
                {
                    Vector3 nextPos = Vector3.Lerp(_startPos, _targetPos, t + LOOK_AHEAD_STEP);
                    nextPos.y += Mathf.Sin((t + LOOK_AHEAD_STEP) * Mathf.PI) * _arcHeight;
                    projectile.transform.forward = (nextPos - currentPos).normalized;
                }
            }
            else
            {
                projectile.ChangeState(Game.Project.Scripts.Core.Projectile.States.ProjectileStates.Impact);
            }
        }

        private Vector3 FindTarget(Vector3 currentPos, Projectile projectile)
        {
            Collider[] colliders = Physics.OverlapSphere(currentPos, TARGET_SEARCH_RADIUS, _ctx.targetMask);

            if (colliders.Length > 0)
            {
                var nearest = colliders
                    .Where(c => c.gameObject != _ctx.owner)
                    .OrderBy(c => Vector3.Distance(currentPos, c.transform.position))
                    .FirstOrDefault();

                return nearest != null ? nearest.transform.position : Vector3.zero;
            }

            return Vector3.zero;
        }
    }
}
