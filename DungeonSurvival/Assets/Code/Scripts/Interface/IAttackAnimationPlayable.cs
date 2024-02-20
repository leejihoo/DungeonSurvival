using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackAnimationPlayable
{
    public void PlayAttackAnimation(NormalMonsterController.StateType prevStateType);
}
