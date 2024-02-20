using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackStateable
{
    public void EnterAttackState();
    public void UpdateAttackState();
    public void ExitAttackState();
}
