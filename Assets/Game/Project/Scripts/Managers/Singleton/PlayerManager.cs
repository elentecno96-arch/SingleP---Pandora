using Game.Project.Scripts.Managers.Systems.PSystems;
using Game.Project.Utillity.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers
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
