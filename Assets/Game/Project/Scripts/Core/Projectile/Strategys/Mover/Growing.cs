using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.Strategys.Mover
{
    /// <summary>
    /// 포물선 이동 방식
    /// </summary>
    public class Growing : IProjectileMover
    {
        private Rigidbody _rb;
        private ParticleSystem[] _childParticles; // 자식 파티클들 저장

        private float _baseScale = 1f;
        private float _maxScale = 4f;
        private float _growthSpeed = 1.5f;
        private float _currentScale;

        public void Init(ProjectileContext context)
        {
            _rb = context.projectile.GetComponent<Rigidbody>();
            _childParticles = context.projectile.GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in _childParticles)
            {
                var main = ps.main;
                main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            }
            if (_rb != null)
            {
                _rb.useGravity = false;
                _rb.isKinematic = false;
                _rb.velocity = context.direction * (context.data.speed * 0.5f);
            }
            _currentScale = _baseScale;
            context.projectile.transform.localScale = Vector3.one * _baseScale;
        }

        public void OnUpdate(Projectile projectile)
        {
            if (_currentScale < _maxScale)
            {
                _currentScale += _growthSpeed * Time.deltaTime;
                projectile.transform.localScale = Vector3.one * _currentScale;
            }
            if (_rb != null && _rb.velocity.sqrMagnitude > 0.1f)
            {
                projectile.transform.rotation = Quaternion.LookRotation(_rb.velocity);
            }
        }
        public void OnFixedUpdate(Projectile projectile) { }
    }
}
