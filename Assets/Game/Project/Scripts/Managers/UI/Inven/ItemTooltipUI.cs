using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Project.Scripts.Managers.UI.Inven
{
    /// <summary>
    /// 인벤토리에서 아이템 툴팁 UI
    /// </summary>
    public class ItemTooltipUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        private RectTransform _rectTransform;

        [SerializeField] private float smoothing = 15f; 
        [SerializeField] private Vector2 offset = new Vector2(25f, -25f);

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            gameObject.SetActive(false);
        }

        private void Update()
        {
            FollowMouse();
        }

        private void FollowMouse()
        {
            Vector2 mousePos = Input.mousePosition;

            float pivotX = mousePos.x > Screen.width * 0.5f ? 1f : 0f;
            float pivotY = mousePos.y > Screen.height * 0.5f ? 1f : 0f;
            _rectTransform.pivot = new Vector2(pivotX, pivotY);

            Vector2 dynamicOffset = new Vector2(
                pivotX == 1f ? -25f : 25f,
                pivotY == 1f ? -25f : 25f
            );

            Vector2 targetPos = mousePos + dynamicOffset;
            _rectTransform.position = Vector2.Lerp(_rectTransform.position, targetPos, Time.unscaledDeltaTime * smoothing);
        }

        public void Show(string itemName)
        {
            if (string.IsNullOrEmpty(itemName)) return;

            _nameText.text = itemName;
            gameObject.SetActive(true);

            _rectTransform.position = (Vector2)Input.mousePosition + offset;

            LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
        }

        public void Hide() => gameObject.SetActive(false);
    }
}
