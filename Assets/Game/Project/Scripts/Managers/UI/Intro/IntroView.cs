using UnityEngine;

namespace Game.Project.Scripts.Managers.UI.Intro
{
    /// <summary>
    /// 나중에 공용 컴포넌트로 뺼 예정 Fade전용
    /// </summary>
    public class IntroView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        public CanvasGroup Group => canvasGroup; 
        public void SetAlpha(float alpha) => canvasGroup.alpha = alpha;
    }
}
