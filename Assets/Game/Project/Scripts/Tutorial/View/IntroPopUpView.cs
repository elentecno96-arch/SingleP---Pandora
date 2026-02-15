using System.Collections;
using TMPro;
using UnityEngine;

public class IntroPopUpView : MonoBehaviour
{
    [SerializeField] private GameObject movePopup;
    [SerializeField] private TextMeshProUGUI countdownText;

    [SerializeField] private const float POPUPDURATION = 5f;

    private Coroutine _hideCoroutine;

    public void ShowMovePopup(float delay = 0f)
    {
        if (_hideCoroutine != null) StopCoroutine(_hideCoroutine);
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
