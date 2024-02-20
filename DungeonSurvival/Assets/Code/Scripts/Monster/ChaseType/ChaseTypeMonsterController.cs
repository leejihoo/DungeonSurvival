public class ChaseTypeMonsterController : NormalMonsterController
{
    #region Functions

    #region MoveState
    
    public override void EnterMoveState()
    {
        _visual.ChangeWarningMark(false);
    }

    public override void UpdateMoveState()
    {
        Patrol();
        Detect();
    }

    public override void ExitMoveState()
    {
        if (_stateMachine.NextState == _stateMachine.NormalMonsterIdleState)
        {
            PlayIdleAnimation(StateType.Move);
            return;
        }
        
        PlayAttackAnimation(StateType.Move);
    }

    #endregion

    #region DieState
    
    public override void EnterDieState()
    {
        OnDie.Invoke();
    }

    #endregion

    #region AttackState
    
    public override void EnterAttackState()
    {
        _visual.ChangeWarningMark(true);
        PlayDetectSound();
    }

    public override void UpdateAttackState()
    {
        Chase();
        Detect();
    }

    public override void ExitAttackState()
    {
        PlayMoveAnimation(StateType.Attack);
    }

    #endregion

    #region IdleState
    
    public override void EnterIdleState()
    {
        StartCoroutine(StartIdleTimer());    
    }

    public override void UpdateIdleState()
    {
        Detect();
    }

    public override void ExitIdleState()
    {
        if (_stateMachine.NextState == _stateMachine.NormalMonsterMoveState)
        {
            PlayMoveAnimation(StateType.Idle);
            return;
        }
        
        PlayAttackAnimation(StateType.Idle);
    }
    
    #endregion
    
    #endregion
}
