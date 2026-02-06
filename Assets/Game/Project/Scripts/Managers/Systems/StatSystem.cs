using Game.Project.Data.Stat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems
{
    /// <summary>
    /// 플레이어의 기본 스텟
    /// </summary>
    public class StatSystem : MonoBehaviour
    {
        [Header("Stat Data")]
        [SerializeField] private Stat baseStat;
        [SerializeField] private Stat additionalStat;  //룬 스탯

        private Stat _cachedCurrentStat;

        /// <summary>
        /// 최종 스탯 (Read-only)
        /// </summary>
        public Stat CurrentStat => _cachedCurrentStat;

        private bool _isInitialized = false;

        private void Awake()
        {
            RefreshStat();
        }

        public void Init()
        {
            if (_isInitialized) return;
            RefreshStat();
            _isInitialized = true;
            Debug.Log("<color=green>StatSystem: 초기화 및 스탯 캐싱 완료</color>");
        }

        /// <summary>
        /// 스킬의 공격 간격
        /// </summary>
        public float GetFinalAttackInterval(float skillCooldown)
        {
            float cdr = _cachedCurrentStat.cooldownReduction;
            float castSpeed = Mathf.Max(0.1f, 1.0f + _cachedCurrentStat.castingSpeed);

            float cooldownAfterCDR = skillCooldown * (1f - cdr);
            return cooldownAfterCDR / castSpeed;
        }

        /// <summary>
        /// 기본 스탯과 추가 스탯을 합산하여 저장
        /// </summary>
        public void RefreshStat()
        {
            _cachedCurrentStat = CalculateTotalStat(baseStat, additionalStat);
        }

        /// <summary>
        /// 외부에서 추가되는 스탯
        /// </summary>
        public void AddAdditionalStat(Stat bonus)
        {
            additionalStat = AddStats(additionalStat, bonus);
            RefreshStat();
        }

        /// <summary>
        /// 추가 스탯을 초기화
        /// </summary>
        public void ResetAdditionalStat()
        {
            additionalStat = new Stat();
            RefreshStat();
        }

        /// <summary>
        /// 2스탯을 단순히 합산하는 용도
        /// </summary>
        private Stat CalculateTotalStat(Stat a, Stat b)
        {
            Stat result = AddStats(a, b);
            result.cooldownReduction = Mathf.Clamp(result.cooldownReduction, 0f, 0.8f);
            result.evasionRate = Mathf.Clamp(result.evasionRate, 0f, 0.75f);
            result.criticalRate = Mathf.Clamp(result.criticalRate, 0f, 1.0f);

            return result;
        }

        /// <summary>
        /// 모든 스탯 필드를 합산
        /// </summary>
        private Stat AddStats(Stat a, Stat b)
        {
            return new Stat
            {
                //레벨
                maxLevel = a.maxLevel + b.maxLevel,
                maxExp = a.maxExp + b.maxExp,

                //생존
                maxHp = a.maxHp + b.maxHp,
                maxMp = a.maxMp + b.maxMp,
                maxMoveSpeed = a.maxMoveSpeed + b.maxMoveSpeed,
                defense = a.defense + b.defense,
                tenacity = a.tenacity + b.tenacity,
                evasionRate = a.evasionRate + b.evasionRate,

                //공격
                atk = a.atk + b.atk,
                criticalRate = a.criticalRate + b.criticalRate,
                criticalDamage = a.criticalDamage + b.criticalDamage,
                cooldownReduction = a.cooldownReduction + b.cooldownReduction,
                castingSpeed = a.castingSpeed + b.castingSpeed,

                pickupRange = a.pickupRange + b.pickupRange
            };
        }
    }
}
