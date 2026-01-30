using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Project._0.Scripts._2.Struct.Stat;

namespace Game.Project._0.Scripts._1.Player
{
    public class PlayerStat : MonoBehaviour
    {
        [SerializeField]
        private Stat baseStat;
        public Stat CurrentStat { get; private set; }
        private void Awake()
        {
            Init();
        }
        public void Init()
        {
            CurrentStat = baseStat;
        }
    }
}
