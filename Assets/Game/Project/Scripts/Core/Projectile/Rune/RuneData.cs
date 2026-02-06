using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.Rune
{
    public enum RuneElement { None, Fire, Ice, Electric, Dark, Earth, Light, Wind }
    public enum ModifierType
    {
        Damage,        //추가 대미지
        Speed,         //투사체 속도
        Duration,      //유지 시간 (LifeTime)
        Cooldown,      //스킬 사용 빈도 감소
        Scale,         //투사체 크기
        CritChance,    //치명타 확률
        CritDamage,    //치명타 대미지
        Acceleration,  //투사체 가속도
        Homing,        //추적 강도
        Radius         //폭발 범위 시너지 용
    }
    [CreateAssetMenu(fileName = "NewRune", menuName = "Projectile/Rune Data")]
    public class RuneData : ScriptableObject
    {
        public string runeName;
        public RuneElement element;
        public ModifierType modifier;

        public float damageMultiplier; //룬 공통 대미지 증가
        public float specialValue; //룬 효과에 따른 증사 값

        [TextArea]
        public string description;
    }
}
