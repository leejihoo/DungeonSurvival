using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveSO : ScriptableObject
{
    public Transform mover;
    public int moveSpeed; 
    public abstract void Move();
}
