using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveStateable
{
    public void EnterMoveState();
    public void UpdateMoveState();
    public void ExitMoveState();
}
