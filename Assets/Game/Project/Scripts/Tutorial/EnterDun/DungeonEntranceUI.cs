using Game.Project.Scripts.Managers.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Project.Scripts.Tutorial.EnterDun
{
    /// <summary>
    /// 던전 진입 확인 UI
    /// </summary>
    public class DungeonEntranceUI : MonoBehaviour
    {
        [SerializeField] private GameObject contentGroup;
        [SerializeField] private Button enterButton;

        private void Awake()
        {
            if (enterButton != null)
                enterButton.onClick.AddListener(OnEnterButtonClicked);

            ShowUI(false);
        }

        public void ShowUI(bool isShow)
        {
            if (contentGroup != null)
                contentGroup.SetActive(isShow);
        }

        private void OnEnterButtonClicked()
        {
            if (GameManager.HasInstance)
            {
                GameManager.Instance.StartDungeon();
            }
            else
            {
                SceneManager.Instance.LoadScene("6. Main");
            }

            ShowUI(false);
        }
    }
}
