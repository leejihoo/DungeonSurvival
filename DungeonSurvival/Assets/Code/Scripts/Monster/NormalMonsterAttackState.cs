using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonsterAttackState : NormalMonsterState
{
    public NormalMonsterAttackState(INormalMonsterController controller)
    {
        _controller = controller;
    }
    
    public override void Enter()
    {
        _controller.EnterAttackState();
    }

    public override void Update()
    {
        _controller.UpdateAttackState();
    }

    public override void Exit()
    {
        _controller.ExitAttackState();
    }
}
