using Game.Project._0.Scripts._0.Managers._1.Systems.GSystems;
using Game.Project.Utillity._0.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project._0.Scripts._0.Managers._0.Singleton
{
    public class GameManager : Singleton<GameManager> 
    {
        public StatSystem Stat { get; private set; }
        public RuneSystem Rune { get; private set; }
        public SkillSystem Skill { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            InitializeSystems();
        }
        private void InitializeSystems()
        {
            Stat = GetComponentInChildren<StatSystem>();
            if (Stat != null) Stat.Init();

            Rune = GetComponentInChildren<RuneSystem>();
            Skill = GetComponentInChildren<SkillSystem>();

            //스탯 => 룬 계산 => 스킬 순서로 초기화
            if (Rune != null) Rune.Init();
            if (Skill != null) Skill.Init();
        }
    }
}
