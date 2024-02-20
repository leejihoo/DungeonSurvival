public abstract class NormalMonsterState : IState
{
    #region Fields

    protected INormalMonsterController _controller;
    
    #endregion

    #region Abstract functions
    
    public abstract void Enter();
    
    public abstract void Update();
    
    public abstract void Exit();
    
    #endregion
}
