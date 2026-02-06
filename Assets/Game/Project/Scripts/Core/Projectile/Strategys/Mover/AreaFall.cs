using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.Strategys.Mover
{
    /// <summary>
    /// ¸ñÇ¥ À§Ä¡ ·£´ý ³«ÇÏ ¹æ½Ä
    /// </summary>
    public class AreaFall : IProjectileMover
    {
        private Rigidbody _rb;
        public void Init(ProjectileContext context)
        {
            _rb = context.projectile.GetComponent<Rigidbody>();
            _rb.useGravity = true;

            Vector3 centerPos = context.target != null ? context.target.transform.position : context.projectile.transform.position + context.direction * 5f;
            Vector2 randomCircle = Random.insideUnitCircle * 3f;
            Vector3 targetPoint = centerPos + new Vector3(randomCircle.x, 0, randomCircle.y);

            Vector3 diff = targetPoint - context.projectile.transform.position;
            float distance = new Vector3(diff.x, 0, diff.z).magnitude;
            float duration = Mathf.Max(distance / context.data.speed, 0.5f);

            float vY = (diff.y / duration) + (0.5f * Mathf.Abs(Physics.gravity.y) * duration);
            _rb.velocity = new Vector3(diff.x / duration, vY, diff.z / duration);
        }
        public void OnUpdate(Projectile projectile)
        {
            if (_rb.velocity.sqrMagnitude > 0.1f)
                projectile.transform.rotation = Quaternion.LookRotation(_rb.velocity);
        }
        public void OnFixedUpdate(Projectile projectile) { }
    }
}
