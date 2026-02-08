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

        private TargetScanner _scanner;
        private Transform _currentTarget;

        private void Awake() => _scanner = GetComponent<TargetScanner>();
        private void Update()
        {
            if (!_scanner.IsTargetValid(_currentTarget))
            {
                _currentTarget = _scanner.GetClosestTarget();
            }
            if (_currentTarget == null) return;

            CheckSkills();
        }

        private void CheckSkills()
        {
            foreach (var skill in equippedSkills)
            {
                if (skill == null) continue;
                if (!_skillTimers.ContainsKey(skill)) _skillTimers[skill] = 0f;
                _skillTimers[skill] += Time.deltaTime;

                float interval = PlayerManager.Instance.Stats.GetFinalAttackInterval(skill.cooldown);

                if (_skillTimers[skill] >= interval)
                {
                    _skillTimers[skill] = 0f;
                    AutoAttack(skill);
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
    }
}
