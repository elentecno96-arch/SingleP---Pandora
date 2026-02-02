using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Skill.Interface
{
    public interface ISkillable
    {
        void Execute();
        bool IsReady { get; }
    }
}
