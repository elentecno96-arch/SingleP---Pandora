using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.UI.Intro;
using Game.Project.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public enum TutorialPhase
    {
        None,
        PlayerAwake,
        MoveTutorial
    }

    [Header("Views")]
    [SerializeField] private IntroView introView;
    [SerializeField] private SelfDialogueView selfDialogueView;
    [SerializeField] private IntroPopUpView introPopUpView;

    [SerializeField] private StoryData introViewData;
    [SerializeField] private StoryData selfDialogueData;

    [SerializeField] private float movePopupDelay = 2f;

    private TutorialPhase _phase = TutorialPhase.None;

    private void Start()
    {
        StartCoroutine(RunTutorial());
    }

    private IEnumerator RunTutorial()
    {
        _phase = TutorialPhase.PlayerAwake;

        introView.Play(introViewData);
        yield return new WaitUntil(() => !introView.IsPlaying);

        yield return new WaitForSeconds(2f);

        selfDialogueView.Play(selfDialogueData);
        yield return new WaitUntil(() => !selfDialogueView.IsPlaying);

        var player = PlayerManager.Instance.CurrentPlayer;
        if (player != null)
        {
            var movement = player.GetComponent<PlayerMovement>();
            if (movement != null)
                movement.enabled = true;
        }

        introPopUpView.ShowMovePopup(movePopupDelay);

        _phase = TutorialPhase.MoveTutorial;
    }
}
