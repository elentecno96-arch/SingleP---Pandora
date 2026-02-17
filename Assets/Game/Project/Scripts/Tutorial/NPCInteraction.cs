using Cinemachine;
using System;
using UnityEngine;

namespace Game.Project.Scripts.Tutorial
{
    /// <summary>
    /// NPC와 상호작용
    /// </summary>
    public class NPCInteraction : MonoBehaviour
    {
        [SerializeField] private StoryData interactionData;
        [SerializeField] private Cinemachine.CinemachineVirtualCamera npcZoomCamera;

        public static event Action<StoryData, CinemachineVirtualCamera> OnNPCInteract;

        public void OnInteract()
        {
            OnNPCInteract?.Invoke(interactionData, npcZoomCamera);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnInteract();
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
