using Game.Project.Data.Stat;
using UnityEngine;
using Game.Project.Scripts.Enemy.EnemySO;

namespace Game.Project.Scripts.Enemy
{
    /// <summary>
    /// 적의 스탯 정보를 담아 보내주는 용도
    /// </summary>
    public class EnemyStatSource : IStatSourceable
    {
        private Stat _finalStat;

        private const float MOVE_SPEED_MULTIPLIER_COEFF = 0.5f; // 스테이지 배율이 이속에 미치는 영향 계수
        private const float MIN_MOVE_SPEED_RATIO = 1.0f;        // 최소 이속 배율
        private const float MAX_MOVE_SPEED_RATIO = 1.5f;        // 최대 이속 배율

        public EnemyStatSource(EnemyData data, float totalMultiplier, float bonusDefense)
        {
            _finalStat = FinalStat(data.baseStat, totalMultiplier, bonusDefense);
        }

        /// <summary>
        /// 적의 최종 스탯
        /// </summary>
        /// <param name="baseStat"></param>
        /// <param name="multiplier"></param>
        /// <param name="bonusDefense"></param>
        /// <returns></returns>
        private Stat FinalStat(Stat baseStat, float multiplier, float bonusDefense)
        {
            Stat result = baseStat;

            result.maxHp *= multiplier;
            result.damage *= multiplier;
            result.defense += bonusDefense;

            result.maxMoveSpeed *= Mathf.Clamp(multiplier * MOVE_SPEED_MULTIPLIER_COEFF, MIN_MOVE_SPEED_RATIO, MAX_MOVE_SPEED_RATIO);

            return result;
        }

        public Stat GetCurrentStat()
        {
            return _finalStat;
        }
    }
}
