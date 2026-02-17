using Game.Project.Scripts.Data.Items;
using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.UI.SkillBulid.View;
using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    [Header("테스트용 에셋 연결")]
    [SerializeField] private ItemData testRuneItem;
    [SerializeField] private ItemData testSkillBookItem;

    void Start()
    {
        // 인벤토리 UI가 로드된 후 실행되도록 약간의 지연을 줍니다.
        Invoke(nameof(DoTest), 0.2f);
    }

    private void DoTest()
    {
        if (PlayerManager.Instance == null || PlayerManager.Instance.Inventory == null)
        {
            Debug.LogError("PlayerManager 또는 InventorySystem을 찾을 수 없습니다!");
            return;
        }

        // 1. 인벤토리 초기화 (기존 데이터 삭제 및 배열 생성)
        PlayerManager.Instance.Inventory.Init();

        // 2. 아이템 추가 (배열의 앞쪽 빈 칸부터 채워짐)
        if (testRuneItem != null)
        {
            PlayerManager.Instance.Inventory.AddItem(testRuneItem, 5);
        }

        if (testSkillBookItem != null)
        {
            PlayerManager.Instance.Inventory.AddItem(testSkillBookItem, 1);
        }

        var view = FindFirstObjectByType<SkillBuildView>();
        if (view != null)
        {
            view.RefreshAll();
            Debug.Log("Inventory UI Refresh 완료!");
        }
        else
        {
            Debug.LogWarning("SkillBuildView를 씬에서 찾을 수 없어 UI가 갱신되지 않았습니다.");
        }
    }
}
