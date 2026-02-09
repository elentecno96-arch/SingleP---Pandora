using Game.Project.Scripts.Core.Projectile.Interface;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.Strategies.Mover
{
    /// <summary>
    /// 직선 이동 방식
    /// 투사체 증가 시 부채꼴로 발사
    /// </summary>
    public class Linear : IProjectileMover
    {
        private ProjectileContext _ctx;

        public void Init(ProjectileContext context, Projectile projectile)
        {
            _ctx = context;

            projectile.transform.localScale = Vector3.one * _ctx.finalScale;

            if (_ctx.direction != Vector3.zero)
                projectile.transform.rotation = Quaternion.LookRotation(_ctx.direction);
        }

        public void OnUpdate(Projectile projectile)
        {
            projectile.transform.position += _ctx.direction * _ctx.finalSpeed * Time.deltaTime;
        }
    }
}
