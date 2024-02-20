public class NormalMonsterDieState : NormalMonsterState
{
    public NormalMonsterDieState(INormalMonsterController controller)
    {
        _controller = controller;
    }

    public override void Enter()
    {
        _controller.EnterDieState();
    }

    public override void Update()
    {
        _controller.UpdateDieState();
    }

    public override void Exit()
    {
        _controller.ExitDieState();
    }
}
