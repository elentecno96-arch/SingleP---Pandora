using Game.Project.Scripts.Managers.Singleton;
using UnityEngine;

namespace Game.Project.Scripts.Player
{
    /// <summary>
    ///  플레이어 생성시 위치
    /// </summary>
    public class PlayerSpawnPoint : MonoBehaviour
    {
        private void Start()
        {
            if (GameManager.HasInstance)
            {
                GameManager.Instance.HandlePlayerSpawn(transform.position);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.5f);
        }
    }
}
