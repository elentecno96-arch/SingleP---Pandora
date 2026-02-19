using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.UI.AbilityTree;
using Game.Project.Scripts.Managers.UI.StatInfo;
using Game.Project.Scripts.Player;
using Game.Project.Scripts.Tutorial.View;
using System.Collections;
using UnityEngine;

namespace Game.Project.Scripts.Tutorial
{
    /// <summary>
    /// 튜토리얼 전용 매니저 느낌의 컨트롤러
    /// 전체적인 흐름과 연출을 담당
    /// </summary>
    public class TutorialController : MonoBehaviour
    {
        public enum TutorialPhase
        {
            None,
            PlayerAwake,
            MoveTutorial,
            MeetNPC,
            SkillHelp,
            LevelUpTutorial,
            CombatEnemy
        }

        [SerializeField] private Game.Project.Scripts.Tutorial.View.IntroView introView;
        [SerializeField] private SelfTalkView selfDialogueView;
        [SerializeField] private PopUpView introPopUpView;
        [SerializeField] private InteractionDialogueView interactionView;

        [SerializeField] private StoryData introViewData;
        [SerializeField] private StoryData selfDialogueData;
        [SerializeField] private StoryData npcDialogueData;
        [SerializeField] private StoryData skillBookData;  // NPC가 마법서 줄 때 대사

        [SerializeField] private Cinemachine.CinemachineVirtualCamera focusCamera;
        [SerializeField] private Cinemachine.CinemachineVirtualCamera npcCamera;

        private bool _isNpcTalking = false;     // NPC 대화 완료 여부
        private bool _onNpcTrigger = false;     // NPC 트리거 진입 여부

        private TutorialPhase _phase = TutorialPhase.None;

        private void Start()
        {
            StartCoroutine(RunTutorial());
        }

        private IEnumerator RunTutorial()
        {
            //스킬 UI 잠금
            if (UiManager.Instance.SkillBuild != null)
                UiManager.Instance.SkillBuild.IsLocked = true;

            SetSystemsLocked(true);

            _phase = TutorialPhase.PlayerAwake;
            EnablePlayerMovement(false);
            introView.Play(introViewData);
            yield return new WaitUntil(() => !introView.IsPlaying);

            yield return new WaitForSeconds(2f);
            selfDialogueView.Play(selfDialogueData);
            yield return new WaitUntil(() => !selfDialogueView.IsPlaying);

            _phase = TutorialPhase.MoveTutorial;
            introPopUpView.ShowPopup("WASD를 눌러 이동할 수 있습니다.", 0f);
            yield return new WaitForSeconds(1f);
            EnablePlayerMovement(true);

            _phase = TutorialPhase.MeetNPC;
            yield return new WaitUntil(() => _onNpcTrigger);
            yield return new WaitUntil(() => _isNpcTalking);

            _phase = TutorialPhase.SkillHelp;

            UiManager.Instance.SkillBuild.IsLocked = false;
            introPopUpView.ShowPopup("기초 마법서를 획득했습니다! (Tab 키를 눌러 장착)", 0f);

            yield return new WaitUntil(() => UiManager.Instance.SkillBuild.IsViewOpen());

            selfDialogueView.Play(skillBookData);
            yield return new WaitUntil(() => !selfDialogueView.IsPlaying);

            EnablePlayerCombat(true);

            var levelSystem = PlayerManager.Instance.levelSystem;
            yield return new WaitUntil(() => levelSystem.CurrentLevel >= 2);
            SetSystemsLocked(false);
            yield return new WaitForSeconds(1.5f);

            introPopUpView.ShowPopup("(P)키로 정보창을, (U)키로 특성창을 열 수 있습니다.", 0f);

            Debug.Log("TutorialController: 시퀀스 가이드 종료. 이제 트리거 기반으로 동작합니다.");
            _phase = TutorialPhase.None;
        }

        // 전투 시스템 활성화를 위한 헬퍼 함수
        private void EnablePlayerCombat(bool enable)
        {
            var player = PlayerManager.Instance.CurrentPlayer;
            if (player != null)
            {
                var combat = player.GetComponent<PlayerCombat>();
                if (combat != null) combat.enabled = enable;
            }
        }

        private void EnablePlayerMovement(bool enable)
        {
            var player = PlayerManager.Instance.CurrentPlayer;
            if (player != null)
            {
                var movement = player.GetComponent<PlayerMovement>();
                if (movement != null) movement.enabled = enable;
            }
        }
        private void SetSystemsLocked(bool lockState)
        {
            if (UiManager.Instance.AbilityTree != null)
                UiManager.Instance.AbilityTree.SetLocked(lockState);

            // PlayerStatPresenter 접근 및 잠금
            if (UiManager.Instance.PlayerStat != null)
                UiManager.Instance.PlayerStat.SetLocked(lockState);
        }

        private void OnEnable()
        {
            NPCInteraction.OnNPCInteract += HandleNPCInteraction;
        }

        private void OnDisable()
        {
            NPCInteraction.OnNPCInteract -= HandleNPCInteraction;
        }

        // NPC가 상호작용 신호를 보냈을 때 실행될 함수
        private void HandleNPCInteraction(StoryData data, Cinemachine.CinemachineVirtualCamera cam)
        {
            if (_phase == TutorialPhase.MeetNPC)
            {
                StartCoroutine(NPCInteractionSequence(data, cam));
            }
        }

        private IEnumerator NPCInteractionSequence(StoryData data, Cinemachine.CinemachineVirtualCamera cam)
        {
            if (cam != null) cam.Priority = 20;

            yield return new WaitForSeconds(1.2f);

            interactionView.Play(data);

            yield return new WaitUntil(() => !interactionView.IsPlaying);

            if (cam != null) cam.Priority = 5;

            yield return new WaitForSeconds(1.0f);

            _isNpcTalking = true;
        }
        public void SetNpcTrigger() => _onNpcTrigger = true;
    }
}
