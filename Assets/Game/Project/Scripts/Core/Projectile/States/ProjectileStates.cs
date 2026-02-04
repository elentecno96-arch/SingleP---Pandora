using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Core.Projectile.States
{
    public static class ProjectileStates
    {
        public static readonly SpawnState Spawn = new SpawnState();
        public static readonly ChargeState Charge = new ChargeState();
        public static readonly FlyState Fly = new FlyState();
        public static readonly ImpactState Impact = new ImpactState();
    }
}
