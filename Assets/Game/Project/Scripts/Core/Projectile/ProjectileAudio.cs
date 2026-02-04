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
            _projectile.OnSpawn += () => Play(_projectile.Context.Data.spawnSfx);
            _projectile.OnCharge += () => Play(_projectile.Context.Data.chargeSfx);
            _projectile.OnFly += () => Play(_projectile.Context.Data.flySfx);
            _projectile.OnImpact += (target) =>
            {
                if (_projectile.Context.Data.impactSfx)
                    AudioManager.Instance.PlaySfxAtPoint(_projectile.Context.Data.impactSfx, transform.position);
            };
        }
        private void Play(AudioClip clip)
        {
            if (clip) AudioManager.Instance.PlaySfx(clip);
        }
    }
}
