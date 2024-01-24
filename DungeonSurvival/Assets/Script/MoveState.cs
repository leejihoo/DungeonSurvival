using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IState
{
    private IController _controller;

    public MoveState(IController controller)
    {
        _controller = controller;
    }
    
    public void Enter()
    {
        _controller.PlayMoveAnimation();
    }

    public void Update()
    {
        _controller.Patrol();
        _controller.Detect();
    }

    public void Exit()
    {
        
    }

}
