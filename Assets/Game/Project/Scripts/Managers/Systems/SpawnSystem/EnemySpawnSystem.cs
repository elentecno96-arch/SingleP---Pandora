using UnityEngine;
using Game.Project.Scripts.Enemy.EnemySO;

namespace Game.Project.Scripts.Managers.Systems.SpawnSystem
{
    /// <summary>
    /// ¸Å´ÏÀú¶û ÆÑÅä¸®¶û ¿¬°áÁ¡
    /// </summary>
    public class EnemySpawnSystem : MonoBehaviour
    {
        [SerializeField] private EnemyFactory _factory;

        public void SpawnAt(EnemyData data, Vector3 position, float multiplier)
        {
            if (_factory == null) _factory = GetComponentInChildren<EnemyFactory>();

            Game.Project.Scripts.Enemy.Enemy newEnemy = _factory.CreateEnemy(data, position, multiplier);
        }
    }
}