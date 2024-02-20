using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIdleStateable
{
    public void EnterIdleState();
    public void UpdateIdleState();
    public void ExitIdleState();
}
