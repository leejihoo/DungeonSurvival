using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyborgKangarooAttackState : IState
{
    private ICyborgKangarooController cyborgKangarooController;

    public CyborgKangarooAttackState(ICyborgKangarooController cyborgKangarooController)
    {
        this.cyborgKangarooController = cyborgKangarooController;
    }
    
    public void Enter()
    {
        cyborgKangarooController.PlayAttackAnimation(NormalMonsterController.StateType.Move);
    }

    public void Update()
    {
        cyborgKangarooController.Chase();
        cyborgKangarooController.Detect();
    }

    public void Exit()
    {
        
    }
}
