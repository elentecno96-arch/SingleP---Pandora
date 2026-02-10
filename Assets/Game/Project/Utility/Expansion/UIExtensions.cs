using System.Collections;
using UnityEngine;

namespace Game.Project.Utility.Extension
{
    public static class UIExtensions
    {
        /// <summary>
        /// CanvasGroup의 알파값을 부드럽게 변경하는 확장 메서드
        /// </summary>
        public static IEnumerator FadeCo(this CanvasGroup canvasGroup, float start, float end, float duration)
        {
            if (canvasGroup == null) yield break;

            float elapsed = 0f;
            canvasGroup.alpha = start;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
                yield return null;
            }

            canvasGroup.alpha = end;
        }
    }
}
