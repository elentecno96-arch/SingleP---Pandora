using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.Project.Scripts.Tutorial.View
{
    /// <summary>
    /// 플레이어 혼잣말
    /// </summary>
    public class SelfTalkView : MonoBehaviour
    {
        [SerializeField] private GameObject TalkPanel;
        [SerializeField] private TextMeshProUGUI speakerNameText;
        [SerializeField] private TextMeshProUGUI selfTalkText;
        [SerializeField] private float TYPESPEED = 0.03f; //상수화

        private StoryData _data;
        private int _index;
        private bool _isTyping;
        private Coroutine typingCo;

        public bool IsPlaying { get; private set; }

        /// <summary>
        /// 대화 시작
        /// </summary>
        /// <param name="data"></param>
        public void Play(StoryData data)
        {
            if (data == null || data.lines.Count == 0) return;

            _data = data;
            _index = 0;
            IsPlaying = true;

            TalkPanel.SetActive(true);
            Time.timeScale = 0f;

            ShowLine();
        }

        private void Update()
        {
            if (!IsPlaying) return;
            // 클릭 또는 스페이스바
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                if (_isTyping)
                    FinishTyping();
                else
                    NextLine();
            }
        }

        /// <summary>
        /// 현재 대사 보여주기
        /// </summary>
        private void ShowLine()
        {
            var line = _data.lines[_index];
            speakerNameText.text = line.speakerName;

            if (typingCo != null) StopCoroutine(typingCo);
            typingCo = StartCoroutine(TypeText(line.dialogue));
        }

        private IEnumerator TypeText(string text)
        {
            _isTyping = true;
            selfTalkText.text = "";

            foreach (char c in text)
            {
                selfTalkText.text += c;
                yield return new WaitForSecondsRealtime(TYPESPEED);
            }

            _isTyping = false;
        }

        //타이핑 중단
        private void FinishTyping()
        {
            if (typingCo != null) StopCoroutine(typingCo);
            selfTalkText.text = _data.lines[_index].dialogue;
            _isTyping = false;
        }

        //다음 대사
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

        //끝
        private void EndStory()
        {
            IsPlaying = false;
            TalkPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
