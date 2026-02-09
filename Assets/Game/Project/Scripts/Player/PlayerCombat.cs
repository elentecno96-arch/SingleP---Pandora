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
        [SerializeField] private List<SkillData> equippedSkills = new List<SkillData>();
        [SerializeField] private Transform firePoint;

        private Dictionary<SkillData, float> _skillTimers = new Dictionary<SkillData, float>();
        private Dictionary<SkillData, float> _cachedIntervals = new Dictionary<SkillData, float>();

        private TargetScanner _scanner;
        private Transform _currentTarget;

        private void Awake()
        {
            _scanner = GetComponent<TargetScanner>();
        }
        private void Start()
        {
            if (PlayerManager.Instance.Stats != null)
            {
                PlayerManager.Instance.Stats.OnStatChanged += RefreshAllSkill;
            }
            RefreshAllSkill();
        }

        private void Update()
        {
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
        private void UpdateTimers()
        {
            foreach (var skill in equippedSkills)
            {
                if (skill == null) continue;
                if (!_skillTimers.ContainsKey(skill)) _skillTimers[skill] = 0f;

                float interval = _cachedIntervals.ContainsKey(skill) ? _cachedIntervals[skill] : 0.1f;

                _skillTimers[skill] = Mathf.Min(_skillTimers[skill] + Time.deltaTime, interval);
            }
        }
        private void CheckSkills()
        {
            foreach (var skill in equippedSkills)
            {
                if (skill == null) continue;

                if (!_cachedIntervals.ContainsKey(skill)) continue;
                float interval = _cachedIntervals[skill];

                if (_skillTimers[skill] >= interval)
                {
                    _skillTimers[skill] -= interval;
                    AutoAttack(skill);
                }
            }
        }
        public void RefreshAllSkill()
        {
            _cachedIntervals.Clear();

            foreach (var skill in equippedSkills)
            {
                if (skill == null) continue;

                float interval = SkillManager.Instance.GetCooldown(skill, PlayerManager.Instance.Stats.CurrentStat);
                _cachedIntervals[skill] = interval;

                if (!_skillTimers.ContainsKey(skill))
                {
                    _skillTimers[skill] = 0f;
                }
            }
        }
        private void AutoAttack(SkillData skill)
        {
            if (_currentTarget == null) return;

            Vector3 attackDir = (_currentTarget.position - firePoint.position).normalized;

            ProjectileContext context = new ProjectileContext
            {
                data = skill,
                owner = gameObject,
                firePosition = firePoint.position,
                direction = attackDir,
            };
            SkillManager.Instance.ApplySkill(context);
        }

        /// <summary>
        /// 에디터 전용 콜백
        /// </summary>
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying) return;
            RefreshAllSkill();

            Debug.Log("인스펙터 변경 감지: 스킬 캐시를 갱신했습니다.");
        }
#endif
    }
}
