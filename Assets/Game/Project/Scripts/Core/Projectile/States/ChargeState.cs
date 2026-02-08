using System.Collections;
using System.Collections.Generic;
using Game.Project.Scripts.Core.Projectile.Interface;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.States
{
    /// <summary>
    /// 스킬을 모으는 상태
    /// </summary>
    public class ChargeState : IProjectileState
    {
        private float _timer = 0f;

        public void Enter(ProjectileContext context, Projectile projectile)
        {
            _timer = 0f;
        }

        public void UpdateState(Projectile projectile)
        {
            _timer += Time.deltaTime;

            if (_timer >= projectile.Context.data.chargeTime)
            {
                projectile.ChangeState(ProjectileStates.Fly);
            }
        }
        public void Exit(ProjectileContext context) { }
    }
}
