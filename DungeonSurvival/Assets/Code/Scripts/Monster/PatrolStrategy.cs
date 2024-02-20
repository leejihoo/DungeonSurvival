using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolStrategy : MoveStrategy
{
    protected float _movementChange;
    protected float _moveDirection;
    protected float _patrolRange;

    public Action OnOverPatrolRange;
    
    public PatrolStrategy(Transform mover, int moveSpeed, NormalMonsterLogic logic, NormalMonsterVisual visual, float patrolRange, StateMachine stateMachine) : base(mover, moveSpeed, logic, visual, stateMachine)
    {
        MoveType = "Patrol";
        _movementChange = 0;
        _moveDirection = 1;
        _patrolRange = patrolRange;
    }

    public override void Move()
    {
        _mover.Translate(_logic.CalculatePatrolMovementChange(ref _movementChange, ref _moveDirection, _moveSpeed, _patrolRange, _mover.GetChild(0), OnOverPatrolRange));
        
    }
}
