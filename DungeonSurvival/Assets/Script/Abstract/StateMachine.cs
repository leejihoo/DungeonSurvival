using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : IStateMachine
{
    public IState CurrentState;
    public IState NextState;
    public void Init(IState initState)
    {
        CurrentState = initState;
        initState.Enter();
    }

    public void TransitionTo(IState nextState)
    {
        if (CurrentState == nextState)
        {
            return;
        }
        NextState = nextState;
        CurrentState.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
    }

    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();    
        }
    }
}
