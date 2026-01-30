using Game.Project._0.Scripts._2.Skill.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project._0.Scripts._2.Skill
{
    public abstract class SkillBase : MonoBehaviour, ISkillable
    {
        [SerializeField]
        protected float coolDown;

        protected float lastUseTime;
        protected float currentAtk;
        public bool IsReady => Time.time >= lastUseTime + coolDown;
        public virtual void Init(float atk)
        {
            currentAtk = atk;
        }
        public abstract void Execute();
    }
}
