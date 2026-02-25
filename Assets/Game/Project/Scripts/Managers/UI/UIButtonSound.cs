using UnityEngine;
using UnityEngine.UI;
using Game.Project.Scripts.Managers.Singleton;

namespace Game.Project.Scripts.Managers.UI
{
    /// <summary>
    /// 버튼 효과음
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class UIButtonSound : MonoBehaviour
    {
        [SerializeField] private AudioClip clickSound;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                if (AudioManager.HasInstance)
                    AudioManager.Instance.PlaySfx(clickSound);
            });
        }
    }
}
