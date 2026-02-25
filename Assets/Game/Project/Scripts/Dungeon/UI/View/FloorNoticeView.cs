using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloorNoticeView : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _gradeText;
    [SerializeField] private TextMeshProUGUI _floorText;

    private Coroutine _noticeEffectCoroutine;

    public void ShowNotice(FloorGradeConfig config, int floor)
    {
        gameObject.SetActive(true);

        _gradeText.text = config.gradeName;
        _gradeText.color = config.gradeColor;
        _floorText.text = $"{floor}F";

        _canvasGroup.alpha = 0f;

        StopAllCoroutines();
        StartCoroutine(NoticeRoutineCo());
    }

    private IEnumerator NoticeRoutineCo()
    {

        float timer = 0f;
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            _canvasGroup.alpha = timer / 0.5f;
            yield return null;
        }
        _canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(2f);

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            _canvasGroup.alpha = timer / 0.5f;
            yield return null;
        }
        _canvasGroup.alpha = 0f;

        gameObject.SetActive(false);
    }
}
