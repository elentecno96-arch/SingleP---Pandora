using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project._0.Scripts._1.Player
{
    public class Player : MonoBehaviour
    {
        private PlayerMovement movement;
        private PlayerStat stat;
        private PlayerCombat combat;
        private PlayerCondition condition;

        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            stat = GetComponent<PlayerStat>();
            combat = GetComponent<PlayerCombat>();
            condition = GetComponent<PlayerCondition>();

            InitAll();
        }
        private void Update()
        {
            MoveInput();
        }
        private void InitAll()
        {
            stat.Init();
            movement.Init(stat.CurrentStat.maxMoveSpeed);

            //condition.Init();
            combat.Init(stat.CurrentStat.atk);
        }
        void MoveInput()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            movement.SetInput(new Vector2(h, v));
        }
    }
}
