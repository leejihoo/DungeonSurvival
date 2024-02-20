using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTypeMonsterVisual : NormalMonsterVisual
{
    public AnimationClip attackAnimationClip;
    public Action OnchargeRaise;
    public float AttackAnimationTime { get; protected set; }
    
    protected override void Awake()
    {
        base.Awake();
        SetAttackAnimationEvent();
        AttackAnimationTime = attackAnimationClip.length;
    }

    private void SetAttackAnimationEvent()
    {
        var events =  attackAnimationClip.events;
        if (events.Length > 0)
        {
            events[0].objectReferenceParameter = this;
        }
    }
    
    // AttackAnimation에 이벤트가 등록되어 있다면 아래 함수가 작동된다. 대소문자 틀리면 작동안함..
    public void OnCharge()
    {
        OnchargeRaise.Invoke();
    }
}
