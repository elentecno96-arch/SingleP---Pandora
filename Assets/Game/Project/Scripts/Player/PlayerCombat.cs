using Game.Project.Data.Stat;
using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Rune;
using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Player.Combat;
using Game.Project.Scripts.Player.Equip;
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

        private Dictionary<SkillSlot, float> _skillTimers = new Dictionary<SkillSlot, float>();
        private Dictionary<SkillSlot, float> _cachedIntervals = new Dictionary<SkillSlot, float>();

        private TargetScanner _scanner;
        private Transform _currentTarget;
        private IStatSourceable _statSource;

        private const float SKILL_ACTIVATION_RANGE = 15.0f;

        private bool _isInitialized = false;

        public void Init(PlayerManager manager)
        {
            if (manager == null || manager.Stats == null || manager.skillEquip == null)
            {
                Debug.LogError("PlayerCombat: 필수 시스템 누락");
                return;
            }

            playerManager = manager;
            _scanner = GetComponent<TargetScanner>();

            _statSource = new PlayerStatSource(manager.Stats);

            playerManager.Stats.OnStatChanged += RefreshAllSkill;
            playerManager.skillEquip.OnSkillChanged += RefreshAllSkill;

            RefreshAllSkill();
            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized) return;

            UpdateTimers();

            if (UiManager.Instance.SkillBuild.IsViewOpen()) return;

            if (!_scanner.IsTargetValid(_currentTarget))
            {
                _currentTarget = _scanner.GetClosestTarget();
            }

            if (_currentTarget != null)
            {
                float distance = Vector3.Distance(transform.position, _currentTarget.position);

                if (distance <= SKILL_ACTIVATION_RANGE) 
                {
                    CheckSkills();
                }
            }
        }

        private void OnDestroy()
        {
            if (PlayerManager.HasInstance && PlayerManager.Instance.Stats != null)
            {
                if (PlayerManager.Instance.Stats != null)
                    PlayerManager.Instance.Stats.OnStatChanged -= RefreshAllSkill;
                if (PlayerManager.Instance.skillEquip != null)
                    PlayerManager.Instance.skillEquip.OnSkillChanged -= RefreshAllSkill;
            }
        }
        /// <summary>
        /// 슬롯에 장착된 스킬의 쿨타임 업데이트
        /// </summary>
        private void UpdateTimers()
        {
            var slots = playerManager.skillEquip.GetSkillSlots();
            foreach (var slot in slots)
            {
                if (slot == null || slot.IsEmpty) continue;

                if (!_skillTimers.ContainsKey(slot)) _skillTimers[slot] = 0f;

                float interval = _cachedIntervals.ContainsKey(slot) ? _cachedIntervals[slot] : 1.0f;
                _skillTimers[slot] = Mathf.Min(_skillTimers[slot] + Time.deltaTime, interval);
            }
        }
        /// <summary>
        /// 장착 시스템을 통해 스킬 체크
        /// </summary>
        private void CheckSkills()
        {
            //UI가 열릴 때 공격 방지
            if (UiManager.Instance.SkillBuild.IsViewOpen()) return;

            foreach (var slot in playerManager.skillEquip.GetSkillSlots())
            {
                if (slot.IsEmpty || !_cachedIntervals.ContainsKey(slot)) continue;

                if (_skillTimers[slot] >= _cachedIntervals[slot])
                {
                    AutoAttack(slot);
                    _skillTimers[slot] = 0f;

                    _cachedIntervals[slot] = SkillManager.Instance.GetCooldown(slot, _statSource);
                }
            }
        }
        public void RefreshAllSkill()
        {
            if (playerManager?.skillEquip == null) return;

            _cachedIntervals.Clear();

            foreach (var slot in playerManager.skillEquip.GetSkillSlots())
            {
                if (slot.IsEmpty) continue;

                float interval = SkillManager.Instance.GetCooldown(slot, _statSource);
                _cachedIntervals[slot] = interval;

                if (!_skillTimers.ContainsKey(slot))
                    _skillTimers[slot] = 0f;
            }
        }
        private void AutoAttack(SkillSlot slot)
        {
            if (_currentTarget == null || slot.IsEmpty) return;

            Vector3 targetAimPoint = _currentTarget.position + Vector3.up * 1.0f;
            Vector3 attackDir = (targetAimPoint - firePoint.position).normalized;

            ProjectileContext context = SkillManager.Instance.CreateContext(slot, gameObject);
            context.firePosition = firePoint.position;
            context.direction = attackDir;
            context.target = _currentTarget.gameObject;
            context.targetMask = LayerMask.GetMask("Enemy");

            List<RuneData> extractedRunes = new List<RuneData>();
            if (slot.equippedRunes != null)
            {
                foreach (var item in slot.equippedRunes)
                {
                    if (item != null && item.runeData != null)
                    {
                        extractedRunes.Add(item.runeData);
                    }
                }
            }
            SkillManager.Instance.ApplySkill(context, extractedRunes, _statSource);
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
