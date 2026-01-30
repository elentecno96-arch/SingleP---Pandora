using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project._0.Scripts._3.Skill.Interface
{
    public interface ISkillable
    {
        void Execute();
        bool IsReady { get; }
    }
}
