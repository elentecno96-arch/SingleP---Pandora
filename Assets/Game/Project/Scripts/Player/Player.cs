using Game.Project.Scripts.Core.Projectile.Rune;
using Game.Project.Scripts.Managers;
using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Player
{
    /// <summary>
    /// 플레이어 객체
    /// </summary>
    public class Player : MonoBehaviour
    {
        private PlayerMovement movement;
        private PlayerCombat combat;

        private bool _isInitialized = false;

        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            combat = GetComponent<PlayerCombat>();
        }
        private void Start()
        {
            Init();
        }
        private void Update()
        {
            MoveInput();
        }
        private void Init()
        {
            if (_isInitialized) return;

            var currentStats = PlayerManager.Instance.Stats.CurrentStat;
            movement.Init(currentStats.maxMoveSpeed);
            _isInitialized = true;
            Debug.Log("Player: 본체 및 하위 컴포넌트 초기화 완료");
        }
        void MoveInput() //추후 인풋 매니저로 관리 예정
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            movement.SetInput(new Vector2(h, v));
        }
    }
}
