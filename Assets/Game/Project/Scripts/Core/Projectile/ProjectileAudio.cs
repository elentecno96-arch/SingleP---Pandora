using Game.Project.Scripts.Managers.Singleton;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile
{
    /// <summary>
    /// 투사체 소리 담당
    /// </summary>
    public class ProjectileAudio : MonoBehaviour
    {
        private Projectile _projectile;

        private void Awake()
        {
            _projectile = GetComponent<Projectile>();
        }
        public void Bind()
        {
            Unbind();

            _projectile.OnSpawn += PlaySpawnSfx;
            _projectile.OnCharge += PlayChargeSfx;
            _projectile.OnFly += PlayFlySfx;
            _projectile.OnImpact += PlayImpactSfx;
        }
        public void Unbind()
        {
            if (_projectile == null) return;

            _projectile.OnSpawn -= PlaySpawnSfx;
            _projectile.OnCharge -= PlayChargeSfx;
            _projectile.OnFly -= PlayFlySfx;
            _projectile.OnImpact -= PlayImpactSfx;
        }
        private void PlaySpawnSfx() => PlayFromPrefab(_projectile.Context.data.spawnSfxPrefab);
        private void PlayChargeSfx() => PlayFromPrefab(_projectile.Context.data.chargeSfxPrefab);
        private void PlayFlySfx() => PlayFromPrefab(_projectile.Context.data.flySfxPrefab);

        private void PlayImpactSfx(GameObject target)
        {
            var prefab = _projectile.Context.data.impactSfxPrefab;
            if (prefab == null || !AudioManager.HasInstance) return;

            AudioManager.Instance.PlaySfxFromPool(prefab, transform.position);
        }

        private void PlayFromPrefab(GameObject prefab)
        {
            if (prefab == null || !AudioManager.HasInstance) return;

            AudioManager.Instance.PlaySfxFromPool(prefab, transform.position);
        }
    }
}
