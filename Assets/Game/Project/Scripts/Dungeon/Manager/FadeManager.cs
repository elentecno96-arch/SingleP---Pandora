using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _fadeCanvasGroup;

    private void Awake()
    {
        if (_fadeCanvasGroup != null) _fadeCanvasGroup.alpha = 0;
    }

    public IEnumerator FadeOutCo(float duration = 1f)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            _fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / duration);
            yield return null;
        }
        _fadeCanvasGroup.alpha = 1;
    }

    public IEnumerator FadeInCo(float duration = 1f)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            _fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, timer / duration);
            yield return null;
        }
        _fadeCanvasGroup.alpha = 0;
        _fadeCanvasGroup.blocksRaycasts = false;
    }
}
