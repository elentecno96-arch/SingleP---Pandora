using UnityEngine;

namespace Game.Project.Scripts.Tutorial
{
    /// <summary>
    /// 튜토리얼 전용 트리거
    /// </summary>
    public class TutorialTrigger : MonoBehaviour
    {
        public enum TriggerType { NPC, EnemyFocus }
        public TriggerType type;

        private TutorialController controller;

        [SerializeField] private NPCInteraction targetNPC;

        private void Start() => controller = FindFirstObjectByType<TutorialController>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var controller = FindFirstObjectByType<TutorialController>();
                if (controller != null)
                {
                    controller.SetNpcTrigger();
                    if (targetNPC != null) targetNPC.OnInteract();
                }
                gameObject.SetActive(false);
            }
        }
    }
}
