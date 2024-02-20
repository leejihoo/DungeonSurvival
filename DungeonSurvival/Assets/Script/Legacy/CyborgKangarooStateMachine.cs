using System;
using UnityEngine;

[Serializable]
public class CyborgKangarooStateMachine
{
    public IState currentState;

    public CyborgKangarooIdleState CyborgKangarooIdleState;
    public CyborgKangarooMoveState CyborgKangarooMoveState;
    public CyborgKangarooDieState CyborgKangarooDieState;
    public CyborgKangarooAttackState CyborgKangarooAttackState;
    
    public CyborgKangarooStateMachine(ICyborgKangarooController cyborgKangarooController)
    {
        CyborgKangarooIdleState = new CyborgKangarooIdleState(cyborgKangarooController);
        CyborgKangarooMoveState = new CyborgKangarooMoveState(cyborgKangarooController);
        CyborgKangarooDieState = new CyborgKangarooDieState(cyborgKangarooController);
        CyborgKangarooAttackState = new CyborgKangarooAttackState(cyborgKangarooController);
    }

    public void Init(IState startingState)
    {
        currentState = startingState;
        startingState.Enter();
    }

    public void TransitionTo(IState nextState)
    {
        if (nextState == currentState)
        {
            return;
        }
        currentState.Exit();
        currentState = nextState;
        nextState.Enter();
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }    
    }
}
