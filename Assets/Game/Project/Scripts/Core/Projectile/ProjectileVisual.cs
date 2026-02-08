using Game.Project.Scripts.Core.Projectile.Rune;
using Game.Project.Scripts.Managers.Singleton;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile
{
    /// <summary>
    /// 투사체 이펙트 담당
    /// </summary>
    public class ProjectileVisual : MonoBehaviour
    {
        private Projectile _projectile;
        private GameObject _chargeFX;
        private GameObject _flyFX;

        private void Awake()
        {
            _projectile = GetComponent<Projectile>();
        }
        public void Bind()
        {
            Unbind();

            _projectile.OnSpawn += PlaySpawnFX;
            _projectile.OnCharge += PlayChargeFX;
            _projectile.OnChargeExit += StopChargeFX;
            _projectile.OnFly += PlayFlyFX;
            _projectile.OnFlyExit += StopFlyFX;
            _projectile.OnImpact += PlayImpactFX;

            ResetVisuals();
        }
        public void Unbind()
        {
            if (_projectile == null) return;

            _projectile.OnSpawn -= PlaySpawnFX;
            _projectile.OnCharge -= PlayChargeFX;
            _projectile.OnChargeExit -= StopChargeFX;
            _projectile.OnFly -= PlayFlyFX;
            _projectile.OnFlyExit -= StopFlyFX;
            _projectile.OnImpact -= PlayImpactFX;
        }
        private void ResetVisuals()
        {
            StopFlyFX();
            StopChargeFX();

            foreach (var ps in GetComponentsInChildren<ParticleSystem>())
            {
                ps.Clear();
            }
        }
        private void PlaySpawnFX()
        {
            if (_projectile.Context.data.spawnEffect == null) return;
            EffectManager.Instance.PlayEffect(_projectile.Context.data.spawnEffect, transform.position, transform.rotation);
        }
        private void PlayChargeFX()
        {
            if (_projectile.Context.data.chargeEffect == null) return;
            _chargeFX = PoolManager.Instance.GetEffect(_projectile.Context.data.chargeEffect, transform.position, transform.rotation, transform);
        }
        private void StopChargeFX()
        {
            if (_chargeFX != null)
            {
                PoolManager.Instance.ReturnEffect(_chargeFX);
                _chargeFX = null;
            }
        }
        private void PlayFlyFX()
        {
            StopFlyFX();
            if (_projectile.Context.data.flyEffect == null) return;
            _flyFX = PoolManager.Instance.GetEffect(_projectile.Context.data.flyEffect, transform.position, transform.rotation, transform);
        }
        private void StopFlyFX()
        {
            if (_flyFX != null)
            {
                PoolManager.Instance.ReturnEffect(_flyFX);
                _flyFX = null;
            }
        }
        private void PlayImpactFX(GameObject target)
        {
            if (_projectile.Context.data.impactEffect == null) return;
            EffectManager.Instance.PlayEffect(_projectile.Context.data.impactEffect, transform.position, Quaternion.identity, 1.5f);
        }
    }
}
