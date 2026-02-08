using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.Rune
{
    public enum RuneElement { None, Fire, Ice, Electric, Dark, Earth, Light, Wind }
    public enum ModifierType
    {
        Damage,         // 대미지
        Speed,          // 투사체 속도
        Range,          // 사거리
        LifeTime,       // 유지 시간
        Cooldown,       // 쿨다운
        Scale,          // 투사체 크기
        CritChance,     // 치명타 확률
        CritDamage,     // 치명타 대미지
        ProjectileCount,// 투사체 개수
        ChargeTime,     // 차지 시간
        Acceleration,   // 가속도
        Homing,         // 유도 강도
        Radius,         // 폭발/영향 범위
        PierceCount,    // 관통 횟수
        BounceCount     // 도탄 횟수
    }
    [CreateAssetMenu(fileName = "NewRune", menuName = "Projectile/Rune Data")]
    public class RuneData : ScriptableObject
    {
        public string runeName;
        public ModifierType modifier;

        public float baseDamageModifier = 0.05f; //룬 장착 시 기본 대미지 배율

        public float specialValue; //특수 속성 수치

        public bool isPercent; //더할 것인가,곱할 것인가

        [TextArea]
        public string description;
    }
}
