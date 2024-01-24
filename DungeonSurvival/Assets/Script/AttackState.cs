using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private IController _controller;

    public AttackState(IController controller)
    {
        _controller = controller;
    }
    
    public void Enter()
    {
        _controller.PlayAttackAnimation();
    }

    public void Update()
    {
        _controller.Chase();
        _controller.Detect();
    }

    public void Exit()
    {
        
    }
}
