using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Project.Scripts.Managers.UI.AbilityTree
{
    /// <summary>
    /// 스탯 노드 뷰
    /// </summary>
    public class AbilityNodeView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public AbilityNote nodeData { get; private set; }
        [SerializeField] private Image _iconImage;
        [SerializeField] private Button _btn;

        public System.Action<AbilityNote, Vector2> OnShowTooltip;
        public System.Action OnHideTooltip;

        public void Setup(AbilityNote data, System.Action<AbilityNote> onClick)
        {
            if (data == null) return; // 데이터가 없으면 중단

            nodeData = data;

            if (_iconImage != null && data.icon != null)
            {
                _iconImage.sprite = data.icon;
            }

            RectTransform rt = GetComponent<RectTransform>();
            if (rt != null) rt.anchoredPosition = data.nodePosition;

            if (_btn != null)
            {
                _btn.onClick.RemoveAllListeners(); // 중복 리스너 방지
                _btn.onClick.AddListener(() => onClick?.Invoke(nodeData));
            }

            UpdateVisual();
        }

        public void UpdateVisual()
        {
            _iconImage.color = nodeData.isUnlocked ? Color.white : new Color(0.5f, 0.5f, 0.5f, 0.8f);
            _btn.interactable = !nodeData.isUnlocked; //해금 완료 노드 클릭 방지
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log($"Enter: {nodeData.nodeName}");
            OnShowTooltip?.Invoke(nodeData, transform.position);
        }

        // 마우스를 뗐을 때
        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log($"Exit: {nodeData.nodeName}");
            OnHideTooltip?.Invoke();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (nodeData != null)
            {
                GetComponent<RectTransform>().anchoredPosition = nodeData.nodePosition;
                gameObject.name = $"Node_{nodeData.nodeName}";
            }
        }
#endif
    }
}
