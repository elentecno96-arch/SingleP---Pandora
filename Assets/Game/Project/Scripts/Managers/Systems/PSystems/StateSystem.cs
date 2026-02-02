using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.Systems.GSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Systems.PSystems
{
    public class StateSystem : MonoBehaviour
    {
        [SerializeField] private float currentHp;
        [SerializeField] private float currentMp;
        [SerializeField] private float currentExp;

        public float CurrentHp => currentHp;
        public float CurrentMp => currentMp;
        public float CurrentExp => currentExp;

        public void Init()
        {
            if (GameManager.Instance != null && GameManager.Instance.Stat != null)
            {
                RefreshMaxStatus();
            }
        }
        public void RefreshMaxStatus()
        {
            var stats = GameManager.Instance.Stat.CurrentStat;

            currentHp = stats.maxHp;
            currentMp = stats.maxMp;
        }
        public void OnDamage(float damage)
        {
            currentHp = Mathf.Max(0, currentHp - damage);
            if (currentHp <= 0) Die();
        }
        private void Die()
        {
            Debug.Log("°ð Á×À½ Ã³¸® ¿¹Á¤");
        }
    }
}
