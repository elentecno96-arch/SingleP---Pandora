using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Project.Scripts.Managers.UI.Dungeon;

public class DungeonEntryTrigger : MonoBehaviour
{
    [SerializeField] private DungeonMediator mediator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (mediator != null)
            {
                mediator.OpenUI();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (mediator != null)
            {
                mediator.CloseUI();
            }
        }
    }
}
