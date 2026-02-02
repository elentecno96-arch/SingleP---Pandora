using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Game.Project.Data.Stat
{
    [Serializable]
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
        public float castingSpeed;
        public float criticalDamage; //크리 대미지 배율 용
        public float criticalRate;
        public float knockBack;
        public static Stat operator +(Stat a, Stat b)
        {
            return new Stat
            {
                maxLevel = a.maxLevel + b.maxLevel,
                maxExp = a.maxExp + b.maxExp,
                maxMoveSpeed = a.maxMoveSpeed + b.maxMoveSpeed,
                cooldownReduction = a.cooldownReduction + b.cooldownReduction,
                pickupRange = a.pickupRange + b.pickupRange,
                maxHp = a.maxHp + b.maxHp,
                maxMp = a.maxMp + b.maxMp,
                defense = a.defense + b.defense,
                tenacity = a.tenacity + b.tenacity,
                evasionRate = a.evasionRate + b.evasionRate,
                atk = a.atk + b.atk,
                castingSpeed = a.castingSpeed + b.castingSpeed,
                criticalDamage = a.criticalDamage + b.criticalDamage,
                criticalRate = a.criticalRate + b.criticalRate,
                knockBack = a.knockBack + b.knockBack
            };
        }
    }
}
