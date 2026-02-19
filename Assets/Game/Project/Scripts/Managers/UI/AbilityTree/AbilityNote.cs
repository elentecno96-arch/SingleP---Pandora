using Game.Project.Data.Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityNode", menuName = "Ability/Node")]
public class AbilityNote : ScriptableObject
{
    public string nodeName;
    public Sprite icon;
    [SerializeField] public AbilityNote abilityNode;

    [TextArea(3, 5)]
    public string description;

    public Stat bonusStat;        
    public int requiredLevel;     
    public bool isUnlocked;       

    public Vector2 nodePosition;
}
