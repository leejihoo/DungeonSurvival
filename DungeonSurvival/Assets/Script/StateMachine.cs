using System;
using UnityEngine;

[Serializable]
public class StateMachine
{
    public IState currentState;

    public IdleState idleState;
    public MoveState moveState;
    public DeadState deadState;
    public AttackState attackState;

    public StateMachine(IController controller)
    {
        idleState = new IdleState(controller);
        moveState = new MoveState(controller);
        deadState = new DeadState(controller);
        attackState = new AttackState(controller);
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
