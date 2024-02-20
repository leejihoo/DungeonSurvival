using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyborgKangarooDieState : IState
{
    private ICyborgKangarooController cyborgKangarooController;

    public CyborgKangarooDieState(ICyborgKangarooController cyborgKangarooController)
    {
        this.cyborgKangarooController = cyborgKangarooController;
    }
    
    public void Enter()
    {
        CyborgKangarooCyborgKangarooController.OnDie.Invoke();
        
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
       
    }
}
