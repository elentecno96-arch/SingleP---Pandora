using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.Project.Scripts.Tutorial.View
{
    /// <summary>
    /// 알림 팝업 UI
    /// </summary>
    public class PopUpView : MonoBehaviour
    {
        [SerializeField] private GameObject movePopup;
        [SerializeField] private TextMeshProUGUI mainContentText;
        [SerializeField] private TextMeshProUGUI countdownText;

        [SerializeField] private const float POPUPDURATION = 3f;

        private Coroutine _hideCoroutine;

        public void ShowPopup(string message, float delay = 0f)
        {
            if (_hideCoroutine != null) StopCoroutine(_hideCoroutine);

            if (mainContentText != null) mainContentText.text = message;

            StartCoroutine(ShowAfterDelay(delay));
        }

        private IEnumerator ShowAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            movePopup.SetActive(true);
            _hideCoroutine = StartCoroutine(HideAfterTimer(POPUPDURATION));
        }

        private IEnumerator HideAfterTimer(float duration)
        {
            float remainingTime = duration;

            while (remainingTime > 0)
            {
                if (countdownText != null)
                {
                    countdownText.text = $"{Mathf.CeilToInt(remainingTime)}초 뒤에 사라집니다.";
                }
                yield return new WaitForSeconds(1f);
                remainingTime -= 1f;
            }

            Hide();
        }

        public void Hide()
        {
            movePopup.SetActive(false);
            _hideCoroutine = null;
        }
    }
}
