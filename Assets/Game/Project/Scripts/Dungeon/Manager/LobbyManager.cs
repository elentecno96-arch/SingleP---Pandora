using Cinemachine;
using Game.Project.Scripts.Managers.Singleton;
using UnityEngine;

/// <summary>
/// 던전의 로비 관리자
/// </summary>
public class LobbyManager : MonoBehaviour
{
    [SerializeField] private Transform lobbySpawnPoint;

    private void Start()
    {
        if (lobbySpawnPoint != null)
        {
            SpawnAndSetup();
        }
    }

    private void SpawnAndSetup()
    {
        GameManager.Instance.PlayerSpawn(lobbySpawnPoint.position);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.rotation = lobbySpawnPoint.rotation;

            var vcam = FindFirstObjectByType<CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.Follow = player.transform;
            }
        }
    }
}
