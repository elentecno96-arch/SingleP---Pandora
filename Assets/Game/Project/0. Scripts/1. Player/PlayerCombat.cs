using Game.Project._0.Scripts._2.Skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project._0.Scripts._1.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private List<SkillBase> skillSlots = new List<SkillBase>();
        private float atk;
        public void Init(float attackDamage)
        {
            this.atk = attackDamage;

            foreach (var skill in skillSlots)
            {
                if (skill != null) skill.Init(this.atk);
            }
        }
        private void Update() => AutoAttack();
        private void AutoAttack()
        {
            if (skillSlots == null) return;

            foreach (var skill in skillSlots)
            {
                if (skill != null && skill.IsReady)
                {
                    skill.Execute();
                }
            }
        }
    }
}
