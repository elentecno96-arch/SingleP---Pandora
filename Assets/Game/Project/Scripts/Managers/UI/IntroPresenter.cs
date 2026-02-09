using Game.Project.Scripts.Managers.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPresenter : MonoBehaviour
{
    [SerializeField] private CanvasGroup logoCanvasGroup;

    [SerializeField] private float startDelay = 1.0f;
    [SerializeField] private float fadeInTime = 3.0f;  //나타나는 시간
    [SerializeField] private float stayTime = 1.5f;    //머무르는 시간
    [SerializeField] private float fadeOutTime = 1.5f; //사라지는 시간

    private bool _isSkipped = false;

    private void Start()
    {
        if (logoCanvasGroup != null)
        {
            logoCanvasGroup.alpha = 0f;
            StartCoroutine(IntroSequenceRoutine());
        }
    }

    private IEnumerator IntroSequenceRoutine()
    {
        yield return new WaitForSeconds(startDelay);
        yield return StartCoroutine(Fade(0f, 1f, fadeInTime));
        yield return new WaitForSeconds(stayTime);
        yield return StartCoroutine(Fade(1f, 0f, fadeOutTime));
        FinishIntro();
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (_isSkipped) yield break;

            elapsed += Time.deltaTime;
            logoCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }
        logoCanvasGroup.alpha = endAlpha;
    }
    private void FinishIntro()
    {
        Debug.Log("Intro: 모든 연출 완료 -> 메인 씬으로 자동 이동");
        GameManager.Instance.StartGame();
    }
}
