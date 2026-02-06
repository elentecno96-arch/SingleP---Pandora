using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Interface;
using Game.Project.Scripts.Core.Projectile.Strategys.Mover;
using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Core.Projectile.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems.SkillSystems
{
    public class SpawnSystem : MonoBehaviour
    {
        public void Spawn(ProjectileContext baseContext)
        {
            int spawnCount = (baseContext.data.movementType == MovementType.Spread)
                ? Mathf.Clamp(baseContext.data.projectileCount, 3, 6) : 1;

            float spreadAngle = 45f;
            float startAngle = -(spreadAngle / 2f);
            float angleStep = (spawnCount > 1) ? spreadAngle / (spawnCount - 1) : 0;

            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 finalDir = baseContext.direction;

                if (baseContext.data.movementType == MovementType.Spread)
                {
                    float currentAngle = startAngle + (angleStep * i);
                    finalDir = Quaternion.Euler(0, currentAngle, 0) * baseContext.direction;
                }

                SpawnIndividual(baseContext, finalDir);
            }
        }

        private void SpawnIndividual(ProjectileContext baseContext, Vector3 direction)
        {
            IProjectileMover mover = CreateMover(baseContext.data.movementType);
            Projectile proj = PoolManager.Instance.GetProjectile(baseContext.data.projectilePrefab);

            if (proj != null)
            {
                ProjectileContext individualContext = CopyContext(baseContext, direction);
                proj.transform.SetPositionAndRotation(baseContext.firePosition, Quaternion.LookRotation(direction));
                proj.Init(individualContext, mover);
            }
        }

        private IProjectileMover CreateMover(MovementType type) => type switch
        {
            MovementType.Growing => new Growing(),
            MovementType.AreaFall => new AreaFall(),
            MovementType.Spread => new Spread(),
            _ => new Linear(),
        };

        private ProjectileContext CopyContext(ProjectileContext origin, Vector3 direction)
        {
            return new ProjectileContext
            {
                data = origin.data,
                owner = origin.owner,
                direction = direction,
                firePosition = origin.firePosition,

                skillDamage = origin.skillDamage,
                skillSpeed = origin.skillSpeed,
                skillLifeTime = origin.skillLifeTime,
                skillCooldown = origin.skillCooldown,
                skillScale = origin.skillScale,
                skillCritChance = origin.skillCritChance,
                skillCritDamage = origin.skillCritDamage,
                skillAcceleration = origin.skillAcceleration,
                skillHomingForce = origin.skillHomingForce,

                isCritical = origin.isCritical,

                synergyExplosionRadius = origin.synergyExplosionRadius,
                synergySlowAmount = origin.synergySlowAmount,
                synergyDefensePen = origin.synergyDefensePen,

                primaryColor = origin.primaryColor,
                secondaryColor = origin.secondaryColor,

                strategies = new List<IProjectileStrategy>(origin.strategies),
                activeSynergy = origin.activeSynergy
            };
        }
    }
}
