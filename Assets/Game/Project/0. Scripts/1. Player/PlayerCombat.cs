using Game.Project._0.Scripts._3.Skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project._0.Scripts._1.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField]
        private List<SkillBase> skillSlots;
        private float atk;
        public void Init(float attackDamage)
        {
            skillSlots = new List<SkillBase>(4);
            this.atk = attackDamage;

            foreach (var skill in skillSlots)
            {
                if (skill != null) skill.Init(this.atk);
            }
        }
        private void Update()
        {
            AutoAttack();
        }
        private void AutoAttack()
        {
            foreach (var skill in skillSlots)
            {
                if (skill == null) continue;
                if (skill.IsReady)
                {
                    skill.Execute();
                }
            }
        }
    }
}
