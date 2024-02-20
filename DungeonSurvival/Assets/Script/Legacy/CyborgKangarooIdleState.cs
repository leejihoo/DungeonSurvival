using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyborgKangarooIdleState : IState
{
    private ICyborgKangarooController cyborgKangarooController;

    public CyborgKangarooIdleState(ICyborgKangarooController cyborgKangarooController)
    {
        this.cyborgKangarooController = cyborgKangarooController;
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
