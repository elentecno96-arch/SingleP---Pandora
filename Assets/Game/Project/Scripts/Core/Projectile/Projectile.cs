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
        private IProjectileMover _mover;
        private bool _isReturned = false;

        [SerializeField] private LayerMask targetMask;

        public event Action OnSpawn;
        public event Action OnCharge;
        public event Action OnFly;
        public event Action<GameObject> OnImpact;
        public event Action OnChargeExit;
        public event Action OnFlyExit;

        public Action<Projectile> OnReturnToPool;

        public ProjectileContext Context => _context;
        public IProjectileMover Mover => _mover;

        public void Init(ProjectileContext context, IProjectileMover mover)
        {
            _isReturned = false;
            ClearAllEvents();

            _context = context;
            _context.projectile = this;
            _mover = mover;

            if (_context.strategies != null)
            {
                foreach (var strategy in _context.strategies)
                {
                    strategy.Apply(_context);
                }
            }
            _mover.Init(_context);

            if (TryGetComponent(out ProjectileVisual visual)) visual.Bind();
            if (TryGetComponent(out ProjectileAudio audio)) audio.Bind();

            transform.localScale = Vector3.one * _context.skillScale;

            ChangeState(ProjectileStates.Spawn);
        }

        public void ChangeState(IProjectileState newState)
        {
            if (_currentState != null)
            {
                if (_currentState == ProjectileStates.Fly) OnFlyExit?.Invoke();
                if (_currentState == ProjectileStates.Charge) OnChargeExit?.Invoke();

                _currentState.Exit(_context);
            }

            _currentState = newState;

            if (_currentState != null)
            {
                if (_currentState == ProjectileStates.Spawn) OnSpawn?.Invoke();
                else if (_currentState == ProjectileStates.Charge) OnCharge?.Invoke();
                else if (_currentState == ProjectileStates.Fly) OnFly?.Invoke();
                else if (_currentState == ProjectileStates.Impact) OnImpact?.Invoke(_context.target);

                _currentState.Enter(_context);
            }
        }
        private void Update() => _currentState?.UpdateState(this);
        private void FixedUpdate()
        {
            if (_currentState is FlyState) _mover?.OnFixedUpdate(this);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (_context == null || _isReturned) return;
            if (other.gameObject == _context.owner) return;

            if ((targetMask.value & (1 << other.gameObject.layer)) != 0)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    if (!other.TryGetComponent(out IDamageable _)) return;
                }

                _context.target = other.gameObject;
                ChangeState(ProjectileStates.Impact);
            }
        }
        public void ReturnToPool()
        {
            if (_isReturned) return;
            _isReturned = true;

            if (_mover is IDisposable disposable) disposable.Dispose();

            ClearAllEvents();
            OnReturnToPool?.Invoke(this);
        }
        private void ClearAllEvents()
        {
            OnSpawn = null;
            OnCharge = null;
            OnFly = null;
            OnImpact = null;
            OnChargeExit = null;
            OnFlyExit = null;
        }
        public void ImpactStateCall(GameObject target)
        {
            OnImpact?.Invoke(target);

            if (target != null && target.TryGetComponent(out IDamageable dmg))
            {
                float finalDmg = _context.skillDamage;
                if (_context.isCritical) finalDmg *= _context.skillCritDamage;

                dmg.TakeDamage(finalDmg);
            }
            SynergyLogic();
        }

        private void SynergyLogic()
        {
            if (_context.activeSynergy == null) return;

            if (_context.activeSynergy.element == Rune.RuneElement.Fire && _context.synergyExplosionRadius > 0)
            {
                ApplyAreaDamage(transform.position, _context.synergyExplosionRadius);
            }
        }
        private void ApplyAreaDamage(Vector3 center, float radius)
        {
            Collider[] hits = Physics.OverlapSphere(center, radius, targetMask);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out IDamageable dmg))
                {
                    dmg.TakeDamage(_context.skillDamage * 0.5f);
                }
            }
        }
    }
}

