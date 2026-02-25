using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.UI.SkillBulid.View;
using UnityEngine;

namespace Game.Project.Scripts.Managers.UI.SkillBulid
{
    /// <summary>
    /// 스킬 빌드(에디터) 중개자
    /// </summary>
    public class SkillBuilderMediator : MonoBehaviour
    {
        [SerializeField] private SkillBuildView view;

        [SerializeField] private AudioClip openSfx;  
        [SerializeField] private AudioClip closeSfx; 

        public bool IsLocked { get; set; } = true;

        // Tab 상호작용
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (IsLocked)
                {
                    Debug.Log("스킬창이 아직 잠겨 있습니다.");
                    return;
                }
                ToggleSkillBuild();
            }
        }

        // 스킬 빌드 UI
        public void ToggleSkillBuild()
        {
            if (view == null) return;

            if (view.IsOpen) Close();
            else Open();
        }

        private void Open()
        {
            view.Show();
            SetCursor(true);

            if (AudioManager.HasInstance && openSfx != null)
            {
                AudioManager.Instance.PlaySfx(openSfx);
            }
        }

        private void Close()
        {
            view.Hide();
            SetCursor(false);

            if (AudioManager.HasInstance && closeSfx != null)
            {
                AudioManager.Instance.PlaySfx(closeSfx);
            }
        }

        // 마우스 상태 설정
        private void SetCursor(bool visible)
        {
            Cursor.visible = visible;
            Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        }

        public bool IsViewOpen() => view != null && view.IsOpen;
    }
}
