using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDieable : IDieAnimationPlayable, IDieSoundPlayable
{
    public void Die();
}
