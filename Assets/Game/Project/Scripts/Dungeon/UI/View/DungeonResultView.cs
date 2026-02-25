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
        public void UpdateResult(int floor)
        {
            if (_lastFloorText != null)
                _lastFloorText.text = $"마지막 도달 층 : {floor}F";
        }
    }
}
