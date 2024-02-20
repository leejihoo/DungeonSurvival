public interface IStateMachine
{
    public void Init(IState initState);
    public void TransitionTo(IState nextState);
    public void Update();
}
