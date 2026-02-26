using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Project.Scripts.Dungeon.UI.View
{
    /// <summary>
    /// 던전 결과창 UI
    /// </summary>
    public class DungeonResultView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _lastFloorText;
        [SerializeField] private TextMeshProUGUI _killCountText;   
        [SerializeField] private TextMeshProUGUI _totalGoldText;   
        [SerializeField] private TextMeshProUGUI _playTimeText;

        [SerializeField] private Button _lobbyButton;
        [SerializeField] private Button _retryButton;

        public event Action OnLobbyRequested;
        public event Action OnRetryRequested;

        public void InitView()
        {
            _lobbyButton.onClick.AddListener(() => OnLobbyRequested?.Invoke());
            _retryButton.onClick.AddListener(() => OnRetryRequested?.Invoke());
        }

        public void Show(bool state) => gameObject.SetActive(state);

        /// <summary>
        /// 마지막으로 도달한 층 정보를 UI에 업데이트
        /// </summary>
        public void UpdateResult(int floor, int kills, int gold, float time)
        {
            if (_lastFloorText != null)
                _lastFloorText.text = $"마지막 도달 층 : <color=yellow>{floor}F</color>";

            if (_killCountText != null)
                _killCountText.text = $"처치한 적 : {kills}마리";

            if (_totalGoldText != null)
                _totalGoldText.text = $"획득한 골드 : {gold:N0} G";

            if (_playTimeText != null)
            {
                int min = Mathf.FloorToInt(time / 60);
                int sec = Mathf.FloorToInt(time % 60);
                _playTimeText.text = $"생존 시간 : {min:00}:{sec:00}";
            }
        }
    }
}
