using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project._0.Scripts._2.Struct.Stat
{
    public struct Stat
    {
        public int maxLevel;
        public float maxExp;

        public float maxMoveSpeed;
        public float cooldownReduction;
        public float pickupRange;

        public float maxHp;
        public float maxMp;
        public float defense;
        public float tenacity; //강인함 (대미지 감소, 또는 상태이상 시간 감소)
        public float evasionRate; //회피율

        public float atk;
        public float castingspeed;
        public float criticalDamage; //크리 대미지 배율 용
        public float criticalRate;
        public float knockback;
    }
}
