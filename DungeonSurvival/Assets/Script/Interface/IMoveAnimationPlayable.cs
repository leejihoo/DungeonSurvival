using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveAnimationPlayable
{
    public void PlayMoveAnimation(NormalMonsterController.StateType prevStateType);
}
