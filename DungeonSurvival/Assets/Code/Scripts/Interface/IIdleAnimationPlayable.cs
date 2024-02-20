using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIdleAnimationPlayable
{
    public void PlayIdleAnimation(NormalMonsterController.StateType prevStateType);
}
