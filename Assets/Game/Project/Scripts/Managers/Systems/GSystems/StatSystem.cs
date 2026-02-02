using Game.Project.Data.Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems.GSystems
{
    public class StatSystem : MonoBehaviour
    {
        [SerializeField] private Stat baseStat;
        private Stat finalStat;

        public Stat CurrentStat => finalStat;
        public void Init()
        {
            finalStat = baseStat;
        }

        public void UpdateFinalStat(Stat bonus)
        {
            finalStat = baseStat + bonus;
        }
    }
}
