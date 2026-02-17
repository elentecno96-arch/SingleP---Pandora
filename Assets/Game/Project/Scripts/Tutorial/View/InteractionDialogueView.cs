using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionDialogueView : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typeSpeed = 0.03f;

    [SerializeField] private Image leftPortrait;
    [SerializeField] private Image rightPortrait;

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
        speakerNameText.text = line.speakerName;

        // 초상화 설정
        SetupPortrait(leftPortrait, line.leftPortrait, line.isLeftSpeaking);
        SetupPortrait(rightPortrait, line.rightPortrait, !line.isLeftSpeaking);

        if (typingCo != null) StopCoroutine(typingCo);
        typingCo = StartCoroutine(TypeText(line.dialogue));
    }

    private void SetupPortrait(Image img, Sprite sprite, bool isSpeaking)
    {
        if (sprite == null)
        {
            img.gameObject.SetActive(false);
            return;
        }

        img.gameObject.SetActive(true);
        img.sprite = sprite;

        img.color = isSpeaking ? Color.white : new Color(0.5f, 0.5f, 0.5f);
        img.transform.localScale = isSpeaking ? Vector3.one * 1.1f : Vector3.one;
    }

    private IEnumerator TypeText(string text)
    {
        _isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(typeSpeed);
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

    private void EndStory()
    {
        IsPlaying = false;
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
