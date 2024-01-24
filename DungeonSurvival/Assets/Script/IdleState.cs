using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private IController _controller;

    public IdleState(IController controller)
    {
        _controller = controller;
    }
    
    public void Enter()
    {
        
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        
    }
}
