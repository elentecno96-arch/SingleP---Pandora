using UnityEngine;

namespace Game.Project.Scripts.Tutorial.EnterDun
{
    /// <summary>
    /// 던전 입장 트리거
    /// </summary>
    public class DungeonEntryTrigger : MonoBehaviour
    {
        [SerializeField] private DungeonEntranceUI entranceUI;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (entranceUI != null)
                {
                    entranceUI.ShowUI(true);

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }      
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (entranceUI != null)
                {
                    entranceUI.ShowUI(false);

                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }   
            }
        }
    }
}
