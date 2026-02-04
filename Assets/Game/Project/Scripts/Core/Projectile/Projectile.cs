using Game.Project.Data.Damage;
using Game.Project.Scripts.Core.Projectile.Interface;
using Game.Project.Scripts.Core.Projectile.States;
using Game.Project.Scripts.Core.Projectile.SO;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile
{
    /// <summary>
    /// 투사체 본채
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        private ProjectileContext _context;
        private IProjectileState _currentState;

        public ProjectileContext Context => _context;

        public event Action OnSpawn;
        public event Action OnCharge;
        public event Action OnFly;
        public event Action<GameObject> OnImpact;

        public event Action OnChargeExit;
        public event Action OnFlyExit;

        public Action<Projectile> OnReturnToPool;
        private bool _isReturned = false;

        public void Init(GameObject owner, Vector3 dir, SkillData data, List<IProjectileStrategy> strategies)
        {
            _isReturned = false;
            _context = new ProjectileContext
            {
                Owner = owner,
                Direction = dir.normalized,
                Data = data,
                Projectile = this
            };
            GetComponent<ProjectileVisual>().Bind();
            GetComponent<ProjectileAudio>().Bind();
            if (strategies != null)
            {
                foreach (var strategy in strategies)
                {
                    strategy.Apply(_context);
                }
            }

            ChangeState(ProjectileStates.Spawn);
        }
        public void ChangeState(IProjectileState newState)
        {
            if (_currentState != null)
            {
                if (_currentState is FlyState) OnFlyExit?.Invoke();
                else if (_currentState is ChargeState) OnChargeExit?.Invoke();

                _currentState.Exit(_context);
            }
            _currentState = newState;

            if (newState is SpawnState) OnSpawn?.Invoke();
            else if (newState is ChargeState) OnCharge?.Invoke();
            else if (newState is FlyState) OnFly?.Invoke();
            else if (newState is ImpactState) OnImpact?.Invoke(_context.Target);
            _currentState.Enter(_context);
        }
        private void Update() => _currentState?.UpdateState(this);
        private void OnTriggerEnter(Collider other)
        {
            if (_context == null || _isReturned) return;
            if (other.gameObject == _context.Owner) return;

            string layerName = LayerMask.LayerToName(other.gameObject.layer);
            if (layerName == "Ground")
            {
                _context.Target = other.gameObject;
                ChangeState(ProjectileStates.Impact);
            }
            else if (layerName == "Enemy")
            {
                if (other.TryGetComponent(out IDamageable dmg))
                {
                    _context.Target = other.gameObject;
                    ChangeState(ProjectileStates.Impact);
                }
            }
        }
        public void ReturnToPool()
        {
            if (_isReturned) return;
            _isReturned = true;

            OnSpawn = null;
            OnCharge = null;
            OnFly = null;
            OnImpact = null;
            OnChargeExit = null;
            OnFlyExit = null;

            if (_context != null)
            {
                _context.ClearEvents();
            }

            OnReturnToPool?.Invoke(this);
        }
    }
}
