using Game.Project._2.Data.Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project._0.Scripts._0.Managers._1.Systems.GSystems
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
