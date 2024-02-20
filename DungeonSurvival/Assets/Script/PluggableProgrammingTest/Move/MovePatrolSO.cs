using System.Collections;
using System.Collections.Generic;
using Script.PluggableProgrammingTest;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/CreateMove/MovePatrol", fileName = "MovePatrol")]
public class MovePatrolSO : MoveSO
{
    public float movementChange;
    public float moveDirection;
    public float patrolRange;
    public override void Move()
    {
        mover.Translate(PluggableMonsterLogic.CalculatePatrolDistance(ref movementChange, ref moveDirection, moveSpeed, patrolRange, mover.GetChild(0)));
    }
}
