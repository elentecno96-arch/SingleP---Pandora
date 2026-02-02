using Game.Project.Scripts.Managers.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        private PlayerMovement movement;
        private PlayerCombat combat;

        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            combat = GetComponent<PlayerCombat>(); 
        }
        private void Start()
        {
            InitAll();
        }
        private void Update()
        {
            MoveInput();
        }
        private void InitAll()
        {
            var currentStats = GameManager.Instance.Stat.CurrentStat;
            movement.Init(currentStats.maxMoveSpeed);
            //combat.Init(currentStats.atk);
        }
        void MoveInput()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            movement.SetInput(new Vector2(h, v));
        }
    }
}
