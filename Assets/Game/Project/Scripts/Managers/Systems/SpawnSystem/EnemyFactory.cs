using Game.Project.Scripts.Enemy;
using Game.Project.Scripts.Enemy.EnemySO;
using Game.Project.Scripts.Managers.Singleton;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems.SpawnSystem
{
    /// <summary>
    /// 적 생산을 담당하는 팩토리
    /// </summary>
    public class EnemyFactory : MonoBehaviour
    {
        private const float ELITE_SPAWN_CHANCE = 0.1f;         // 엘리트 출현 확률 (10%)
        private const float ELITE_STAT_MULTIPLIER = 2.0f;      // 엘리트 스탯 배율 (2배)
        private const float ELITE_BONUS_DEFENSE = 5.0f;        // 엘리트 추가 방어력
        private const float ELITE_SCALE_MULTIPLIER = 1.3f;     // 엘리트 크기 배율 (1.3배)
        private const float NORMAL_SCALE_MULTIPLIER = 1.0f;    // 일반 크기 배율

        /// <summary>
        /// 적 생산
        /// </summary>
        /// <param name="data"></param>
        /// <param name="position"></param>
        /// <param name="stageMultiplier"></param>
        /// <param name="forceElite"></param>
        /// <returns></returns>
        public Enemy.Enemy CreateEnemy(EnemyData data, Vector3 position, float stageMultiplier, bool forceElite = false)
        {
            Enemy.Enemy enemy = PoolManager.Instance.GetEnemy(data.prefab);
            if (enemy == null) return null;

            enemy.transform.position = position;
            enemy.gameObject.SetActive(true);

            bool isElite = forceElite || (Random.value < ELITE_SPAWN_CHANCE);

            float totalMultiplier = stageMultiplier * (isElite ? ELITE_STAT_MULTIPLIER : 1.0f);
            float bonusDefense = isElite ? ELITE_BONUS_DEFENSE : 0f;

            EnemyContext ctx = new EnemyContext(data, enemy.gameObject);
            EnemyStatSource statSource = new EnemyStatSource(data, totalMultiplier, bonusDefense);

            ctx.SetupStat(statSource.GetCurrentStat(), isElite);

            enemy.Init(ctx);

            //엘리트 크기 설정
            float finalScale = isElite ? ELITE_SCALE_MULTIPLIER : NORMAL_SCALE_MULTIPLIER;
            enemy.transform.localScale = data.prefab.transform.localScale * finalScale;

            return enemy;
        }
    }
}
