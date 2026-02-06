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
            StopFlyFX();
            StopChargeFX();
            if (_projectile.Context.activeSynergy != null)
                ApplyPalette(gameObject, _projectile.Context.activeSynergy.flyPalette);

            _projectile.OnSpawn += PlaySpawnFX;
            _projectile.OnCharge += PlayChargeFX;
            _projectile.OnChargeExit += StopChargeFX;
            _projectile.OnFly += PlayFlyFX;
            _projectile.OnFlyExit += StopFlyFX;
            _projectile.OnImpact += PlayImpactFX;
        }

        private void PlaySpawnFX()
        {
            if (_projectile.Context.data.spawnEffect == null) return;
            GameObject fx = EffectManager.Instance.PlayEffect(_projectile.Context.data.spawnEffect, transform.position, transform.rotation);

            if (_projectile.Context.activeSynergy != null)
                ApplyPalette(fx, _projectile.Context.activeSynergy.spawnPalette);
        }

        private void PlayChargeFX()
        {
            if (_projectile.Context.data.chargeEffect == null) return;
            _chargeFX = PoolManager.Instance.GetObject(_projectile.Context.data.chargeEffect, transform);

            if (_projectile.Context.activeSynergy != null)
                ApplyPalette(_chargeFX, _projectile.Context.activeSynergy.chargePalette);
        }
        private void StopChargeFX()
        {
            if (_chargeFX != null)
            {
                PoolManager.Instance.ReturnObject(_chargeFX);
                _chargeFX = null;
            }
        }
        private void PlayFlyFX()
        {
            StopFlyFX();
            if (_projectile.Context.data.flyEffect == null) return;
            _flyFX = PoolManager.Instance.GetObject(_projectile.Context.data.flyEffect, transform);

            if (_projectile.Context.activeSynergy != null)
                ApplyPalette(_flyFX, _projectile.Context.activeSynergy.flyPalette);
        }
        private void StopFlyFX()
        {
            if (_flyFX != null)
            {
                PoolManager.Instance.ReturnObject(_flyFX);
                _flyFX = null;
            }
        }
        private void PlayImpactFX(GameObject target)
        {
            if (_projectile.Context.data.impactEffect == null) return;
            GameObject fx = EffectManager.Instance.PlayEffect(_projectile.Context.data.impactEffect, transform.position, Quaternion.identity, 1.5f);

            if (_projectile.Context.activeSynergy != null)
            {
                ApplyPalette(fx, _projectile.Context.activeSynergy.impactPalette);
            }
        }
        private void ApplyPalette(GameObject obj, ColorPalette palette)
        {
            if (obj == null) return;

            Color[] colors = new Color[3];
            colors[0] = palette.primary; colors[0].a = 1f;
            colors[1] = palette.secondary; colors[1].a = 1f;
            colors[2] = palette.tertiary; colors[2].a = 1f;

            var allParticles = obj.GetComponentsInChildren<ParticleSystem>(true);

            for (int i = 0; i < allParticles.Length; i++)
            {
                int colorIndex = i % colors.Length;
                var main = allParticles[i].main;
                main.startColor = colors[colorIndex];
            }
        }
    }
}
