using Game.Project.Scripts.Enemy.EnemySO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyGroup_", menuName = "Dungeon/Enemy Group")]
public class EnemyGroupSO : ScriptableObject
{
    public FloorGrade grade;            // Normal, Unique, Legend
    public List<EnemyData> enemyList;   // 해당 층에서 나올 몬스터들
}
