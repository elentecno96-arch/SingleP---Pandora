using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Project.Scripts.Tutorial.View
{
    /// <summary>
    /// 인트로 대사와 배경을 보여주는 View
    /// </summary>
    public class IntroView : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI speakerNameText;
        [SerializeField] private TextMeshProUGUI dialogueText;

        [SerializeField] private const float TYPESPEED = 0.03f;

        private StoryData _data;
        private int _index;
        private bool _isTyping;
        private Coroutine typingCo;

        public bool IsPlaying { get; private set; }

        public void Play(StoryData data)
        {
            if (data == null || data.lines.Count == 0) return;

            _data = data;
            _index = 0;
            IsPlaying = true;

            dialoguePanel.SetActive(true);
            backgroundImage.gameObject.SetActive(true);
            Time.timeScale = 0f;

            ShowLine();
        }

        private void Update()
        {
            if (!IsPlaying) return;
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                if (_isTyping)
                    FinishTyping();
                else
                    NextLine();
            }
        }

        private void ShowLine()
        {
            var line = _data.lines[_index];
            backgroundImage.sprite = line.background;
            speakerNameText.text = line.speakerName;

            if (typingCo != null) StopCoroutine(typingCo);
            typingCo = StartCoroutine(TypeText(line.dialogue));
        }

        private IEnumerator TypeText(string text)
        {
            _isTyping = true;
            dialogueText.text = "";

            foreach (char c in text)
            {
                dialogueText.text += c;
                yield return new WaitForSecondsRealtime(TYPESPEED);
            }

            _isTyping = false;
        }

        private void FinishTyping()
        {
            if (typingCo != null) StopCoroutine(typingCo);
            dialogueText.text = _data.lines[_index].dialogue;
            _isTyping = false;
        }

        private void NextLine()
        {
            _index++;
            if (_index >= _data.lines.Count)
            {
                EndStory();
                return;
            }

            ShowLine();
        }

        // 배경 페이드 아웃 코루틴
        private IEnumerator FadeOutBackground(float duration)
        {
            Color c = backgroundImage.color;
            float startAlpha = c.a;
            float t = 0f;

            while (t < duration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(startAlpha, 0f, t / duration);
                backgroundImage.color = c;
                yield return null;
            }

            c.a = 0f;
            backgroundImage.color = c;
            backgroundImage.gameObject.SetActive(false);
        }

        private void EndStory()
        {
            IsPlaying = false;
            StartCoroutine(FadeOutBackground(1.5f));
            dialoguePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
