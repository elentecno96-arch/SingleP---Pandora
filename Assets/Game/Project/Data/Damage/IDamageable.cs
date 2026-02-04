using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Data.Damage
{
    public interface IDamageable
    {
        void TakeDamage(float damage);
    }
}
