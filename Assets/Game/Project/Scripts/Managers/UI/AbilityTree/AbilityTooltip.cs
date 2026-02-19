using TMPro;
using UnityEngine;

namespace Game.Project.Scripts.Managers.UI.AbilityTree
{
    /// <summary>
    /// 스탯 트리 노드의 툴팁 
    /// </summary>
    public class AbilityTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descText;
        [SerializeField] private Vector2 _offset = new Vector2(0, 0); // 마우스와 툴팁 사이 간격

        private RectTransform _rectTransform;
        private Canvas _canvas;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            gameObject.SetActive(false);
        }

        private void Update()
        {
            FollowMouse();
        }

        private void FollowMouse()
        {
            Vector2 mousePos = Input.mousePosition;

            _rectTransform.position = mousePos + _offset;

            float rightEdge = Screen.width - (_rectTransform.rect.width * _canvas.scaleFactor);
            float topEdge = Screen.height - (_rectTransform.rect.height * _canvas.scaleFactor);

            Vector3 pos = _rectTransform.position;
            if (pos.x > rightEdge) pos.x = mousePos.x - _rectTransform.rect.width - _offset.x;
            if (pos.y > topEdge) pos.y = mousePos.y - _rectTransform.rect.height - _offset.y;

            _rectTransform.position = pos;
        }

        public void Show(AbilityNote data)
        {
            if (data == null) return;

            _nameText.text = data.nodeName;
            if (!string.IsNullOrEmpty(data.description))
            {
                _descText.text = data.description;
            }

            gameObject.SetActive(true);
            FollowMouse();
        }

        public void Hide() => gameObject.SetActive(false);
    }
}
