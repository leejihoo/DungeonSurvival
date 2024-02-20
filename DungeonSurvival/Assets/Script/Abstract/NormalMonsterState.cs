using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NormalMonsterState : IState
{
    protected INormalMonsterController _controller;

    public abstract void Enter();


    public abstract void Update();


    public abstract void Exit();
}
