using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveStrategy : IMoveAnimationPlayable, IMoveable
{
    public string MoveType { get; protected set; }
    protected Transform _mover;
    protected int _moveSpeed;
    protected NormalMonsterLogic _logic;
    protected NormalMonsterVisual _visual;
    protected StateMachine _stateMachine;

    protected MoveStrategy(Transform mover, int moveSpeed, NormalMonsterLogic logic, NormalMonsterVisual visual, StateMachine stateMachine)
    {
        _mover = mover;
        _moveSpeed = moveSpeed;
        _logic = logic;
        _visual = visual;
        _stateMachine = stateMachine;
    }

    // visual에서 정의해서 controller에서 재생할 지 전략 패턴 객체에서 재생시킬 지 고민
    public virtual void PlayMoveAnimation(NormalMonsterController.StateType prevStateType)
    {
        _visual.PlayMoveAnimation(prevStateType);
    }
    
    public abstract void Move();
}
