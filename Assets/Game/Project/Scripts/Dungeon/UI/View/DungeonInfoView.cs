using TMPro;
using UnityEngine;

public class DungeonInfoView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI floorText;
    [SerializeField] private TextMeshProUGUI gradeText;
    [SerializeField] private TextMeshProUGUI multiplierText;

    public void UpdateInfo(int floor, FloorGradeConfig config, float finalMultiplier)
    {
        floorText.text = $"{floor}F";

        gradeText.text = config.gradeName;
        gradeText.color = config.gradeColor;

        multiplierText.text = $"x{finalMultiplier:F1}";
    }

    public void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }
}
