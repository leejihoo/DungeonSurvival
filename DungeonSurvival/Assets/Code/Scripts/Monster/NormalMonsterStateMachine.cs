public class NormalMonsterStateMachine : StateMachine
{
    public NormalMonsterDieState NormalMonsterDieState;
    public NormalMonsterIdleState NormalMonsterIdleState;
    public NormalMonsterAttackState NormalMonsterAttackState;
    public NormalMonsterMoveState NormalMonsterMoveState;

    public NormalMonsterStateMachine(INormalMonsterController controller)
    {
        NormalMonsterDieState = new NormalMonsterDieState(controller);
        NormalMonsterIdleState = new NormalMonsterIdleState(controller);
        NormalMonsterAttackState = new NormalMonsterAttackState(controller);
        NormalMonsterMoveState = new NormalMonsterMoveState(controller);
    }
}
