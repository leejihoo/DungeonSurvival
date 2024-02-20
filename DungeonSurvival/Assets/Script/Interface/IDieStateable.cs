using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDieStateable
{
    public void EnterDieState();
    public void UpdateDieState();
    public void ExitDieState();
}
