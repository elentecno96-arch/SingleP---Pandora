using Game.Project.Scripts.Managers.Singleton;
using Game.Project.Scripts.Managers.UI.Dungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 축복 트리거
/// </summary>
public class BlessingTrigger : MonoBehaviour
{
    [SerializeField] private DungeonMediator mediator;
    private bool _hasUsedInThisFloor = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_hasUsedInThisFloor || !other.CompareTag("Player")) return;

        if (other.CompareTag("Player"))
        {
            mediator.OpenBlessingShop();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mediator.CloseBlessingShop();
        }
    }
}
