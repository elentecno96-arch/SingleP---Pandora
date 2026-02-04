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
        [SerializeField] private Stat baseStat;
        [SerializeField] private Stat additionalStat;
        public Stat CurrentStat => GetCalculatedStat(baseStat, additionalStat);

        private bool _isInitialized = false;

        public void Init()
        {
            if (_isInitialized) return;
            _isInitialized = true;
            Debug.Log("StatSystem: 초기화 완료");
        }
        //계산의 경우 분리 예정
        private Stat GetCalculatedStat(Stat a, Stat b)
        {
            Stat result = new Stat();

            //단순 연산
            result.maxLevel = a.maxLevel + b.maxLevel;
            result.maxExp = a.maxExp + b.maxExp;
            result.maxMoveSpeed = a.maxMoveSpeed + b.maxMoveSpeed;
            result.pickupRange = a.pickupRange + b.pickupRange;
            result.maxHp = a.maxHp + b.maxHp;
            result.maxMp = a.maxMp + b.maxMp;
            result.defense = a.defense + b.defense;
            result.tenacity = a.tenacity + b.tenacity;
            result.atk = a.atk + b.atk;
            result.criticalDamage = a.criticalDamage + b.criticalDamage;

            //%연산,제한
            result.cooldownReduction = Mathf.Clamp(a.cooldownReduction + b.cooldownReduction, 0, 0.8f);
            result.evasionRate = Mathf.Clamp(a.evasionRate + b.evasionRate, 0, 0.75f);
            result.criticalRate = Mathf.Clamp(a.criticalRate + b.criticalRate, 0, 1.0f);

            return result;
        }

        public void AddAdditionalStat(Stat bonus)
        {
            additionalStat = GetCalculatedStat(additionalStat, bonus);
        }

        public void ResetAdditionalStat()
        {
            additionalStat = new Stat();
        }
    }
}
