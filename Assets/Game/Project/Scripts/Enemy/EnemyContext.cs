using Game.Project.Data.Stat;
using UnityEngine;
using Game.Project.Scripts.Enemy.EnemySO;

namespace Game.Project.Scripts.Enemy
{
    /// <summary>
    /// 적의 동적 데이터 모음
    /// </summary>
    public class EnemyContext
    {
        public EnemyData data;          // 원본 데이터 (SO)
        public Stat currentStat;        // 현재 적용된 최종 스탯
        public float currentHp;         // 실시간 현재 체력

        public GameObject owner;        // 이 데이터를 소유한 몬스터
        public GameObject target;       // 현재 추적/공격 중인 대상

        public Vector3 spawnPosition;   // 스폰 시점의 좌표
        public bool isElite;            // 엘리트 몬스터 여부

        /// <summary>
        /// 데이터와 소유자를 연결
        /// </summary>
        public EnemyContext(EnemyData data, GameObject owner)
        {
            this.data = data;
            this.owner = owner;

            if (owner != null)
                this.spawnPosition = owner.transform.position;
        }

        /// <summary>
        /// 최종 스텟
        /// </summary>
        public void SetupStat(Stat finalStat, bool isElite)
        {
            this.currentStat = finalStat;
            this.currentHp = finalStat.maxHp; 
            this.isElite = isElite;
        }
    }
}
