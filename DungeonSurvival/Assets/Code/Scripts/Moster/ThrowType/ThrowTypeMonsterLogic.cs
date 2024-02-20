using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTypeMonsterLogic : NormalMonsterLogic
{
    public bool IsInAttackRange(int attackRange, Transform target)
    {
        var distance = VectorToTarget(target).magnitude;
 
        return distance <= attackRange;
    }
}
