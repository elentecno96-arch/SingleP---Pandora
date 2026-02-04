using Game.Project.Utillity.Generic;
using Game.Project.Scripts.Managers.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Singleton
{
    /// <summary>
    /// 플레이어의 상태 담당하는 매니저
    /// </summary>
    public class PlayerManager : Singleton<PlayerManager>
    {
        public StatSystem Stats { get; private set; }
        public StateSystem State { get; private set; }
        private bool _isInitialized = false;

        public void Init()
        {
            if (_isInitialized) return;
            Stats = GetComponentInChildren<StatSystem>();
            State = GetComponentInChildren<StateSystem>();

            Stats?.Init();
            State?.Init();

            _isInitialized = true;
            Debug.Log("PlayerManager: 초기화 완료");
        }
    }
}
