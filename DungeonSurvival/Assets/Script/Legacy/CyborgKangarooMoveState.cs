using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyborgKangarooMoveState : IState
{
    private ICyborgKangarooController cyborgKangarooController;

    public CyborgKangarooMoveState(ICyborgKangarooController cyborgKangarooController)
    {
        this.cyborgKangarooController = cyborgKangarooController;
    }
    
    public void Enter()
    {
        cyborgKangarooController.PlayMoveAnimation(NormalMonsterController.StateType.Attack);
    }

    public void Update()
    {
        cyborgKangarooController.Patrol();
        cyborgKangarooController.Detect();
    }

    public void Exit()
    {
        
    }

}
