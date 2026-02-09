using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.SO;
using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Player.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Player
{
    /// <summary>
    /// 플레이어 전투 
    /// </summary>
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;

        private PlayerManager playerManager;

        private Dictionary<SkillData, float> _skillTimers = new Dictionary<SkillData, float>();
        private Dictionary<SkillData, float> _cachedIntervals = new Dictionary<SkillData, float>();

        private TargetScanner _scanner;
        private Transform _currentTarget;

        private bool _isInitialized = false;

        public void Init(PlayerManager manager)
        {
            playerManager = manager;
            _scanner = GetComponent<TargetScanner>(); 

            playerManager.Stats.OnStatChanged += RefreshAllSkill;
            playerManager.skillEquip.OnSkillChanged += RefreshAllSkill;

            RefreshAllSkill();
            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized) return;

            UpdateTimers(); //쿨타임 계속 업데이트

            if (!_scanner.IsTargetValid(_currentTarget))
            {
                _currentTarget = _scanner.GetClosestTarget();
            }
            if (_currentTarget != null)
            {
                CheckSkills();
            }
        }
        private void OnDestroy()
        {
            if (PlayerManager.HasInstance && PlayerManager.Instance.Stats != null)
            {
                PlayerManager.Instance.Stats.OnStatChanged -= RefreshAllSkill;
            }
        }
        /// <summary>
        /// 슬롯에 장착된 스킬의 쿨타임 업데이트
        /// </summary>
        private void UpdateTimers()
        {
            if (playerManager.skillEquip == null) return;
            var slots = playerManager.skillEquip.GetSkillSlots();
            if (slots == null) return;

            foreach (var slot in slots)
            {
                if (slot == null || slot.IsEmpty) continue;

                SkillData skill = slot.skillData;
                if (skill == null) continue;

                if (!_skillTimers.ContainsKey(skill)) _skillTimers[skill] = 0f;

                float interval = _cachedIntervals.ContainsKey(skill) ? _cachedIntervals[skill] : 0.1f;
                _skillTimers[skill] = Mathf.Min(_skillTimers[skill] + Time.deltaTime, interval);
            }
        }
        /// <summary>
        /// 장착 시스템을 통해 스킬 체크
        /// </summary>
        private void CheckSkills()
        {
            if (playerManager.skillEquip == null) return;

            foreach (var slot in playerManager.skillEquip.GetSkillSlots())
            {
                if (slot.IsEmpty) continue;

                SkillData skill = slot.skillData;

                if (!_cachedIntervals.ContainsKey(skill)) continue;

                float interval = _cachedIntervals[skill];

                if (_skillTimers[skill] >= interval)
                {
                    _skillTimers[skill] -= interval;
                    AutoAttack(slot); //슬롯을 넘겨줌
                }
            }
        }
        public void RefreshAllSkill()
        {
            if (playerManager == null || playerManager.skillEquip == null) return;

            _cachedIntervals.Clear();

            foreach (var slot in playerManager.skillEquip.GetSkillSlots())
            {
                if (slot.IsEmpty) continue;

                SkillData skill = slot.skillData;

                float interval = SkillManager.Instance.GetCooldown(skill, playerManager.Stats.CurrentStat);
                _cachedIntervals[skill] = interval;

                if (!_skillTimers.ContainsKey(skill))
                {
                    _skillTimers[skill] = 0f;
                }
            }
        }
        private void AutoAttack(SkillSlot slot)
        {
            if (_currentTarget == null || slot.IsEmpty) return;

            Vector3 attackDir = (_currentTarget.position - firePoint.position).normalized;

            ProjectileContext context = SkillManager.Instance.CreateContext(slot, gameObject);

            context.firePosition = firePoint.position;
            context.direction = attackDir;

            SkillManager.Instance.ApplySkill(context);
        }

        /// <summary>
        /// 에디터 전용 콜백
        /// </summary>
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying) return;

            if (playerManager == null) playerManager = GetComponent<PlayerManager>();

            if (playerManager == null || playerManager.skillEquip == null) return;

            RefreshAllSkill();

            Debug.Log("인스펙터 변경 감지: 스킬 캐시를 갱신했습니다.");
        }
#endif
    }
}
