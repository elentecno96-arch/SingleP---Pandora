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
        private void PlaySpawnSfx() => Play(_projectile.Context.data.spawnSfx);
        private void PlayChargeSfx() => Play(_projectile.Context.data.chargeSfx);
        private void PlayFlySfx() => Play(_projectile.Context.data.flySfx);
        private void PlayImpactSfx(GameObject target)
        {
            if (!_projectile.Context.data.impactSfx) return;
            if (!AudioManager.HasInstance) return;

            AudioManager.Instance.PlaySfxAtPoint(
                _projectile.Context.data.impactSfx,
                transform.position
            );
        }
        private void Play(AudioClip clip)
        {
            if (clip) AudioManager.Instance.PlaySfx(clip);
        }
    }
}
