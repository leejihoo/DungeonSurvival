using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonsterMoveState : NormalMonsterState
{
    public NormalMonsterMoveState(INormalMonsterController controller)
    {
        _controller = controller;
    }
    
    public override void Enter()
    {
        _controller.EnterMoveState();
    }

    public override void Update()
    {
        _controller.UpdateMoveState();
    }

    public override void Exit()
    {
        _controller.ExitMoveState();
    }
}
