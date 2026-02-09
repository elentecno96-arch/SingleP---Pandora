using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.SO;
using Game.Project.Scripts.Managers.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems.SkillSystems
{
    public class SpawnSystem : MonoBehaviour
    {
        //Spin
        private const float SPREAD_ANGLE_STEP = 15f;         // 발사체 사이의 각도 간격

        //Growing
        private const float GROWING_BASE_SCALE = 1.5f;       // 기본 목표 크기
        private const float GROWING_BONUS_PER_STACK = 0.7f;  // 추가 발사체 개수당 크기 보너스
        private const float GROWING_MAX_SCALE_LIMIT = 5.0f;  // 최대 확장 크기 제한

        //Rifle
        private const float RIFLE_BASE_INTERVAL = 0.08f;     //기본 연사

        private MoverFactory moverFactory;

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
                SpawnCopyProjectile(prototype, i, totalCount, spawnedProjectiles);
            }
            return spawnedProjectiles;
        }

        private void ApplyDistribution(ProjectileContext ctx, ProjectileContext proto, int index, int total)
        {
            float startAngle = -(SPREAD_ANGLE_STEP * (total - 1)) / 2f;
            float finalAngle = startAngle + (SPREAD_ANGLE_STEP * index);

            switch (proto.data.movementType)
            {
                case MovementType.Linear:
                case MovementType.Rifle:
                case MovementType.Bounce:
                    ctx.direction = proto.direction;
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
                    float calculatedScale = GROWING_BASE_SCALE + (total - 1) * GROWING_BONUS_PER_STACK;
                    ctx.finalScale = Mathf.Min(calculatedScale * proto.finalScale, GROWING_MAX_SCALE_LIMIT);
                    ctx.direction = proto.direction;
                    break;
            }
        }

        private IEnumerator FireRifleRoutine(ProjectileContext prototype)
        {
            int totalCount = prototype.finalProjectileCount;

            float dynamicInterval = RIFLE_BASE_INTERVAL * totalCount;

            for (int i = 0; i < totalCount; i++)
            {
                SpawnCopyProjectile(prototype, i, totalCount, null);

                if (i < totalCount - 1)
                {
                    yield return new WaitForSeconds(dynamicInterval);
                }
            }
        }
        private void SpawnCopyProjectile(ProjectileContext prototype, int index, int total, List<Projectile> list)
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
