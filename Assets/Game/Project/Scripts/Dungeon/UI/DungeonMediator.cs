using Game.Project.Scripts.Managers.Singleton;
using UnityEngine;
using Game.Project.Scripts.Dungeon.Manager;
using Game.Project.Scripts.Dungeon.UI.View;

namespace Game.Project.Scripts.Managers.UI.Dungeon
{
    /// <summary>
    /// 던전 입장 안내 UI를 관리하는 중재자
    /// </summary>
    public class DungeonMediator : MonoBehaviour
    {
        [SerializeField] private DungeonEntranceView entranceView;
        [SerializeField] private DungeonStageManager stageManager;
        [SerializeField] private DungeonResultView resultView;

        private int _currentPageIndex = 0;
        private void Awake()
        {
            if (GameManager.HasInstance)
            {
                GameManager.Instance.RegisterDungeonMediator(this);
                Debug.Log("<color=cyan>[DungeonMediator] GameManager에 성공적으로 등록됨</color>");
            }
        }

        private void Start()
        {
            entranceView.OnNextPageRequested += NextPage;
            entranceView.OnPrevPageRequested += PrevPage;
            entranceView.OnEnterRequested += EnterDungeon;

            resultView.InitView();
            resultView.OnLobbyRequested += GoToMain;
            resultView.OnRetryRequested += Retry;

            resultView.Show(false);
            entranceView.Show(false);
        }

        public void OpenUI()
        {
            _currentPageIndex = 0;
            entranceView.UpdatePage(_currentPageIndex);
            entranceView.Show(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void CloseUI()
        {
            entranceView.Show(false);

            // Cursor.lockState = CursorLockMode.Locked;
            // Cursor.visible = false;
        }

        private void NextPage()
        {
            if (_currentPageIndex < entranceView.PageCount - 1)
            {
                _currentPageIndex++;
                entranceView.UpdatePage(_currentPageIndex);
            }
        }

        private void PrevPage()
        {
            if (_currentPageIndex > 0)
            {
                _currentPageIndex--;
                entranceView.UpdatePage(_currentPageIndex);
            }
        }

        /// <summary>
        /// 던전 로비에서 던전 입장
        /// </summary>
        private void EnterDungeon()
        {
            SetPlayerControl(false);
            CloseUI();

            UiManager.Instance.UnlockFullSystem();

            if (stageManager != null)
            {
                PlayerPrefs.SetInt("HasEnteredDungeon", 1);
                PlayerPrefs.Save();

                stageManager.ProceedToNextFloor();
            }
        }

        /// <summary>
        /// 사망 시 GameManager가 호출할 메서드
        /// </summary>
        public void OpenResultUI(int lastFloor)
        {
            if (resultView == null)
                resultView = GetComponentInChildren<DungeonResultView>(true);

            if (resultView != null)
            {
                resultView.gameObject.SetActive(true);
                resultView.UpdateResult(lastFloor);
                resultView.Show(true);
            }

            UiManager.Instance.CloseAllSystemUI();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        /// <summary>
        /// 재 도전시 로비로 
        /// </summary>
        private void Retry()
        {
            Time.timeScale = 1f;

            resultView.Show(false);

            SetPlayerControl(true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (stageManager != null)
            {
                stageManager.LoadAndStart();
            }
        }

        /// <summary>
        /// 메인으로
        /// </summary>
        private void GoToMain()
        {
            Time.timeScale = 1f;

            resultView.Show(false);

            if (GameManager.HasInstance)
            {
                GameManager.Instance.StartGame();
            }
        }

        /// <summary>
        /// 플레이어의 이동 및 전투 시스템을 켜고 끕니다.
        /// </summary>
        private void SetPlayerControl(bool enable)
        {
            if (!PlayerManager.HasInstance || PlayerManager.Instance.CurrentPlayer == null) return;

            var player = PlayerManager.Instance.CurrentPlayer;

            if (player.TryGetComponent<PlayerMovement>(out var movement))
            {
                movement.enabled = enable;

                if (!enable && player.TryGetComponent<Rigidbody>(out var rb))
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }

            if (PlayerManager.Instance.Combat != null)
            {
                PlayerManager.Instance.Combat.enabled = enable;
            }
        }
    }
}