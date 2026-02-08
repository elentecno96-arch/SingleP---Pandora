using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Interface;
using Game.Project.Scripts.Core.Projectile.SO;
using Game.Project.Scripts.Core.Projectile.Strategys.Mover;
using Game.Project.Scripts.Managers.Singleton;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems.SkillSystems
{
    public class SpawnSystem : MonoBehaviour
    {
        [SerializeField] private MoverFactory moverFactory;

        private void Awake()
        {
            moverFactory = new MoverFactory();
        }
        public List<Projectile> CreateProjectiles(ProjectileContext prototype)
        {
            List<Projectile> spawnedProjectiles = new List<Projectile>();
            int totalCount = prototype.finalProjectileCount;

            if (prototype.data.movementType == MovementType.Rifle)
            {
                StartCoroutine(FireRifleRoutine(prototype));
                return spawnedProjectiles;
            }
            int actualSpawnCount = (prototype.data.movementType == MovementType.Growing) ? 1 : totalCount;
            for (int i = 0; i < actualSpawnCount; i++)
            {
                SpawnSingleProjectile(prototype, i, totalCount, spawnedProjectiles);
            }
            return spawnedProjectiles;
        }

        private void ApplyDistribution(ProjectileContext ctx, ProjectileContext proto, int index, int total)
        {
            float angleStep = 15f;
            float startAngle = -(angleStep * (total - 1)) / 2f;
            float finalAngle = startAngle + (angleStep * index);

            switch (proto.data.movementType)
            {
                case MovementType.Linear:
                case MovementType.Rifle:
                case MovementType.Bounce:
                    ctx.direction = Quaternion.Euler(0, finalAngle, 0) * proto.direction;
                    break;

                case MovementType.Spin:
                    ctx.direction = Quaternion.Euler(0, finalAngle, 0) * proto.direction;
                    ctx.direction.y = 0;
                    ctx.direction.Normalize();
                    break;

                case MovementType.Arcane:
                    ctx.direction = proto.direction;
                    break;

                case MovementType.Growing:
                    float baseTargetScale = 1.5f;
                    float scaleBonusPerCount = 0.7f;
                    float calculatedScale = baseTargetScale + (total - 1) * scaleBonusPerCount;
                    ctx.finalScale = Mathf.Min(calculatedScale * proto.finalScale, 5.0f);
                    ctx.direction = proto.direction;
                    break;
            }
        }
        
        private IEnumerator FireRifleRoutine(ProjectileContext prototype)
        {
            int totalCount = prototype.finalProjectileCount;
            float interval = 0.08f;

            for (int i = 0; i < totalCount; i++)
            {
                SpawnSingleProjectile(prototype, i, totalCount, null);
                yield return new WaitForSeconds(interval);
            }
        }
        private void SpawnSingleProjectile(ProjectileContext prototype, int index, int total, List<Projectile> list)
        {
            ProjectileContext individualContext = prototype.Clone();
            ApplyDistribution(individualContext, prototype, index, total);

            Projectile proj = PoolManager.Instance.GetProjectile(prototype.data.projectilePrefab);
            if (proj != null)
            {
                proj.Init(individualContext, moverFactory.Create(prototype.data.movementType));
                list?.Add(proj);
            }
        }
    }
}
