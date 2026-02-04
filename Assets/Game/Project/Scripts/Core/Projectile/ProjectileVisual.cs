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
            StopFlyFX();
            StopChargeFX();

            _projectile.OnSpawn += PlaySpawnFX;
            _projectile.OnCharge += PlayChargeFX;
            _projectile.OnChargeExit += StopChargeFX;
            _projectile.OnFly += PlayFlyFX;
            _projectile.OnFlyExit += StopFlyFX;
            _projectile.OnImpact += PlayImpactFX;
        }

        private void PlaySpawnFX() =>
            EffectManager.Instance.PlayEffect(_projectile.Context.Data.spawnEffect, transform.position, transform.rotation);

        private void PlayChargeFX() =>
            _chargeFX = PoolManager.Instance.GetGameObject(_projectile.Context.Data.chargeEffect, transform);

        private void StopChargeFX()
        {
            if (_chargeFX != null)
            {
                PoolManager.Instance.ReleaseGameObject(_chargeFX);
                _chargeFX = null;
            }
        }
        private void PlayFlyFX()
        {
            StopFlyFX();

            if (_projectile.Context.Data.flyEffect != null)
            {
                _flyFX = PoolManager.Instance.GetGameObject(_projectile.Context.Data.flyEffect, transform);
            }
        }

        private void StopFlyFX()
        {
            if (_flyFX != null)
            {
                PoolManager.Instance.ReleaseGameObject(_flyFX);
                _flyFX = null;
            }
        }
        private void PlayImpactFX(GameObject target)
        {
            if (_projectile.Context.Data.impactEffect == null) return;

            EffectManager.Instance.PlayEffect(
                _projectile.Context.Data.impactEffect,
                transform.position,
                Quaternion.identity,
                1.5f
            );
        }
    }
}
