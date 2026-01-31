using Game.Project._0.Scripts._0.Managers._1.Systems.PSystems;
using Game.Project.Utillity._0.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project._0.Scripts._0.Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        public StateSystem State { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            SetupSystems();
        }
        private void SetupSystems()
        {
            State = GetComponentInChildren<StateSystem>();

            if (State != null) State.Init();
        }
    }
}
