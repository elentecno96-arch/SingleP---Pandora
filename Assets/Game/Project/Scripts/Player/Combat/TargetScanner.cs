using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Player.Combat
{
    /// <summary>
    /// 적을 감지하는 스캐너
    /// </summary>
    public class TargetScanner : MonoBehaviour
    {
        [SerializeField] private float detectRadius = 8f;
        [SerializeField] private LayerMask enemyLayer;

        public Transform GetClosestTarget()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius, enemyLayer);
            if (hits.Length == 0) return null;

            Transform closest = null;
            float minDist = float.MaxValue;

            foreach (var hit in hits)
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = hit.transform;
                }
            }
            return closest;
        }

        public bool IsTargetValid(Transform target)
        {
            if (target == null) return false;
            float dist = Vector3.Distance(transform.position, target.position);
            return target.gameObject.activeInHierarchy && dist <= detectRadius;
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, detectRadius);
        }
    }
}
