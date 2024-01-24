using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : IState
{
    private IController _controller;

    public DeadState(IController controller)
    {
        _controller = controller;
    }
    
    public void Enter()
    {
        NormalMosterController.OnDie.Invoke();
        
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
       
    }
}
