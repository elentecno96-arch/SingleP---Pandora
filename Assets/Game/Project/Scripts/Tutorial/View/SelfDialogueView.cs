using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelfDialogueView : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI selfDialogueText;
    [SerializeField] private float typeSpeed = 0.03f;

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

        if (typingCo != null) StopCoroutine(typingCo);
        typingCo = StartCoroutine(TypeText(line.dialogue));
    }

    private IEnumerator TypeText(string text)
    {
        _isTyping = true;
        selfDialogueText.text = "";

        foreach (char c in text)
        {
            selfDialogueText.text += c;
            yield return new WaitForSecondsRealtime(typeSpeed);
        }

        _isTyping = false;
    }

    private void FinishTyping()
    {
        if (typingCo != null) StopCoroutine(typingCo);
        selfDialogueText.text = _data.lines[_index].dialogue;
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
