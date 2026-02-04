using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Core.Projectile.Interface;
using Game.Project.Scripts.Core.Projectile.SO;

namespace Game.Project.Scripts.Player
{
    /// <summary>
    /// 플레이어 전투 
    /// </summary>
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private float detectRadius = 8f;
        [SerializeField] private LayerMask enemyLayer;

        [SerializeField] private SkillData currentSkillData;
        [SerializeField] private Transform firePoint;

        // 현재 장착된 룬(전략) 리스트
        private List<IProjectileStrategy> _equippedRunes = new List<IProjectileStrategy>();

        private Transform _currentTarget;
        private float _attackTimer;

        private void Update()
        {
            _attackTimer += Time.deltaTime;

            if (!hasCheckTarget())
            {
                _currentTarget = null;
                FindTarget();
                return;
            }
            if (_attackTimer >= currentSkillData.cooldown)
            {
                _attackTimer = 0f;
                Attack();
            }
        }
        public void AddRune(IProjectileStrategy rune)
        {
            if (!_equippedRunes.Contains(rune))
                _equippedRunes.Add(rune);
        }

        private void Attack()
        {
            if (_currentTarget == null || currentSkillData == null) return;

            Vector3 dir = (_currentTarget.position - firePoint.position).normalized;
            dir = (dir + Random.insideUnitSphere * 0.05f).normalized;

            SkillManager.Instance.FireProjectile(
                currentSkillData.projectilePrefab,
                firePoint.position,
                dir,
                gameObject,
                currentSkillData,
                new List<IProjectileStrategy>(_equippedRunes)
            );
        }
        private void FindTarget()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius, enemyLayer);
            if (hits.Length == 0) return;
            _currentTarget = GetClosestTarget(hits);
        }

        private Transform GetClosestTarget(Collider[] targets)
        {
            Transform closest = null;
            float minDist = float.MaxValue;
            foreach (var col in targets)
            {
                if (col == null) continue;
                float dist = Vector3.Distance(transform.position, col.transform.position);
                if (dist < minDist) { minDist = dist; closest = col.transform; }
            }
            return closest;
        }

        private bool hasCheckTarget()
        {
            if (_currentTarget == null) return false;
            return Vector3.Distance(transform.position, _currentTarget.position) <= detectRadius;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, detectRadius);
        }
    }
}
