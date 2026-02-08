using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.Strategys.Mover
{
    /// <summary>
    /// 포물선 크기 증가 스킬
    /// </summary>
    public class Growing : IProjectileMover
    {
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

            _targetPos = FindTarget(projectile.transform.position);

            if (_targetPos == Vector3.zero)
            {
                _targetPos = _startPos + (_ctx.direction * _ctx.finalRange);
            }
            _duration = 1.2f;
            _elapsedTime = 0f;

            projectile.transform.localScale = Vector3.one * (_ctx.finalScale * 0.2f);

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

                float currentScale = Mathf.Lerp(_ctx.finalScale * 0.2f, _ctx.finalScale, t);
                projectile.transform.localScale = Vector3.one * currentScale;

                if (t < 0.99f)
                {
                    Vector3 nextPos = Vector3.Lerp(_startPos, _targetPos, t + 0.01f);
                    nextPos.y += Mathf.Sin((t + 0.01f) * Mathf.PI) * _arcHeight;
                    projectile.transform.forward = (nextPos - currentPos).normalized;
                }
            }
            else
            {
                projectile.ChangeState(Game.Project.Scripts.Core.Projectile.States.ProjectileStates.Impact);
            }
        }

        private Vector3 FindTarget(Vector3 currentPos)
        {
            float searchRadius = 20f;
            Collider[] colliders = Physics.OverlapSphere(currentPos, searchRadius, LayerMask.GetMask("Enemy"));

            if (colliders.Length > 0)
            {
                var nearest = colliders
                    .OrderBy(c => Vector3.Distance(currentPos, c.transform.position))
                    .First();

                return nearest.transform.position;
            }

            return Vector3.zero;
        }
    }
}
