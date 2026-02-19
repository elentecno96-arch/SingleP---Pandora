using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityLineView : MonoBehaviour
{
    [SerializeField] private Image _lineImage;
    private AbilityNote _targetNodeData;

    public void Connect(RectTransform startRT, RectTransform endRT, AbilityNote targetData)
    {
        _targetNodeData = targetData;
        if (_lineImage == null) _lineImage = GetComponent<Image>();

        RectTransform myRT = GetComponent<RectTransform>();

        Vector2 startPos = startRT.anchoredPosition;
        Vector2 endPos = endRT.anchoredPosition;
        myRT.anchoredPosition = (startPos + endPos) / 2f;

        Vector2 direction = endPos - startPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        myRT.rotation = Quaternion.Euler(0, 0, angle);

        float distance = direction.magnitude;
        myRT.sizeDelta = new Vector2(distance, 10f); 

        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (_targetNodeData == null || _lineImage == null) return;

        _lineImage.color = _targetNodeData.isUnlocked ? Color.yellow : new Color(0.3f, 0.3f, 0.3f, 0.5f);
    }
}
