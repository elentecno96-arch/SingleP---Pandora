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
        private bool _hasImpacted = false;

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
            _hasImpacted = false;
            ClearAllEvents();

            _context = context;
            _mover = mover;

            transform.position = _context.firePosition;
            if (_context.direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(_context.direction);

            if (TryGetComponent(out ProjectileVisual visual)) visual.Bind();
            if (TryGetComponent(out ProjectileAudio audio)) audio.Bind();

            _mover.Init(_context, this);

            transform.localScale = Vector3.one * _context.finalScale;

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

                _currentState.Enter(_context, this);
            }
        }
        private void Update() => _currentState?.UpdateState(this);
        private void OnTriggerEnter(Collider other)
        {
            if (_isReturned || _context == null) return;
            if (other.gameObject == _context.owner) return;

            if ((targetMask.value & (1 << other.gameObject.layer)) != 0)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    if (!other.TryGetComponent(out IDamageable _)) return;
                }

                bool shouldImpact = true;

                if (_mover is IProjectileHitable hitHandler)
                {
                    shouldImpact = hitHandler.OnHit(this, other);
                }
                if (shouldImpact)
                {
                    if (_hasImpacted) return;
                    _hasImpacted = true;
                    _context.target = other.gameObject;
                    ChangeState(ProjectileStates.Impact);
                }
                else
                {
                    ImpactStateCall(other.gameObject);
                }
            }
        }
        public void ReturnToPool()
        {
            if (_isReturned) return;
            _isReturned = true;

            StopAllCoroutines();
            if (_mover is IDisposable disposable) disposable.Dispose();
            if (TryGetComponent(out ProjectileVisual visual)) visual.Unbind();

            _context = null;
            _mover = null;
            _currentState = null;

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
                float damageValue = _context.finalDamage;

                if (_context.isCritical)
                {
                    damageValue *= _context.finalCritDamage;
                }
                dmg.TakeDamage(damageValue);
            }
        }
    }
}

