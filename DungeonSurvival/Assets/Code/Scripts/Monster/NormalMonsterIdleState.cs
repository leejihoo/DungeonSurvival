using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonsterIdleState : NormalMonsterState
{
    public NormalMonsterIdleState(INormalMonsterController controller)
    {
        _controller = controller;
    }

    public override void Enter()
    {
        _controller.EnterIdleState();
    }

    public override void Update()
    {
        _controller.UpdateIdleState();
    }

    public override void Exit()
    {
        _controller.ExitIdleState();
    }
}
