using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseStrategy : MoveStrategy
{
    protected Transform _target;
    
    public ChaseStrategy(Transform mover, Transform target, int moveSpeed, NormalMonsterLogic logic, NormalMonsterVisual visual, StateMachine stateMachine) : base(mover, moveSpeed, logic, visual, stateMachine)
    {
        MoveType = "Chase";
        _target = target;
    }
    
    public override void Move()
    {
        _mover.position = _logic.MoveToTarget(_target, _moveSpeed);
    }
}
