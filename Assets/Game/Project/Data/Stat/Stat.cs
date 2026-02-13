using System;

namespace Game.Project.Data.Stat
{
    /// <summary>
    /// 속성 수치
    /// </summary>
    [Serializable]
    public struct Stat
    {
        public int maxLevel;
        public float maxExp;

        public float maxHp;
        public float maxMoveSpeed;
        public float defense;
        public float damage;
        public float castingSpeed;
        public float critDamage; //크리 대미지 배율 용
        public float critChance;
    }
}
