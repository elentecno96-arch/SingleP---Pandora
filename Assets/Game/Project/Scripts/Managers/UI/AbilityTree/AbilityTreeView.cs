using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.UI.AbilityTree
{
    /// <summary>
    /// Ω∫≈» ∆Æ∏Æ ∫‰
    /// </summary>
    public class AbilityTreeView : MonoBehaviour
    {
        [SerializeField] private GameObject _nodePrefab;
        [SerializeField] private GameObject _linePrefab;

        [SerializeField] private RectTransform _contentParent;

        private List<AbilityNodeView> _nodeViews = new List<AbilityNodeView>();
        private List<AbilityLineView> _lineViews = new List<AbilityLineView>();

        public System.Action<AbilityNote> OnNodeEnter;
        public System.Action OnNodeExit;

        public void CreateTree(List<AbilityNote> nodes, System.Action<AbilityNote> onNodeClick)
        {
            ClearTree();

            foreach (var node in nodes)
            {
                if (node == null) continue;

                var nodeGo = Instantiate(_nodePrefab, _contentParent);
                var view = nodeGo.GetComponent<AbilityNodeView>();
                view.Setup(node, onNodeClick);

                view.OnShowTooltip = (data, pos) => OnNodeEnter?.Invoke(data);
                view.OnHideTooltip = () => OnNodeExit?.Invoke();

                _nodeViews.Add(view);
            }

            // ∂Û¿Œ ª˝º∫
            foreach (var view in _nodeViews)
            {
                if (view.nodeData.abilityNode != null)
                {
                    var parentView = _nodeViews.Find(v => v.nodeData == view.nodeData.abilityNode);

                    if (parentView != null)
                    {
                        var lineGo = Instantiate(_linePrefab, _contentParent);
                        lineGo.transform.SetAsFirstSibling();

                        var lineView = lineGo.GetComponent<AbilityLineView>();
                        lineView.Connect(parentView.GetComponent<RectTransform>(), view.GetComponent<RectTransform>(), view.nodeData);
                        _lineViews.Add(lineView);
                    }
                }
            }
        }

        public void RefreshTree()
        {
            _nodeViews.ForEach(v => v.UpdateVisual());
            _lineViews.ForEach(l => l.UpdateVisual());
        }

        private void ClearTree()
        {
            foreach (Transform child in _contentParent) Destroy(child.gameObject);
            _nodeViews.Clear();
            _lineViews.Clear();
        }
    }
}
