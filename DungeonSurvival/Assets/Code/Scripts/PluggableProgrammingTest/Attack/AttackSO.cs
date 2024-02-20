using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackSO : ScriptableObject
{
    public AudioClip attackSound;
    public Transform target;
    public Transform attacker;
    public abstract void Attack();
    
    public void PlayHitSound(Vector3 targetPos)
    {
        SoundManager.Instance.PlaySfxAt(targetPos,attackSound,false);
        
    }
}
