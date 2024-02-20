using System.Collections;
using System.Collections.Generic;
using Script.PluggableProgrammingTest;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/CreateMove/MoveChase", fileName = "MoveChase")]
public class MoveChaseSO : MoveSO
{
    public Transform target;
    public override void Move()
    {
        mover.position = PluggableMonsterLogic.MoveToTarget(target, mover, moveSpeed);
    }
}
