public abstract class StateMachine : IStateMachine
{
    #region Fields
    
    public IState CurrentState;
    public IState NextState;
    
    #endregion

    #region Functions
    
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
    
    #endregion
}
