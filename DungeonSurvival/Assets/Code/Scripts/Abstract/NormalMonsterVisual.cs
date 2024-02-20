using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Script.PluggableProgrammingTest;

public abstract class NormalMonsterVisual : MonoBehaviour
{
    protected Animator animator;
    protected Image warningMark;
    protected TMP_Text hp;
    
    protected int _detectHash;
    protected int _patrolOverHash;
    [SerializeField] private Material outlineMaterial;
    private Material baseMaterial;
    
    protected virtual void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        if (warningMark == null)
        {
            warningMark = transform.parent.Find("NormalMonsterCanvas").Find("WarningMark").GetComponent<Image>();
        }

        if (hp == null)
        {
            hp = transform.parent.Find("NormalMonsterCanvas").Find("Hp").GetComponent<TMP_Text>();
        }
        
        _detectHash = Animator.StringToHash("IsDetectPlayer");
        _patrolOverHash = Animator.StringToHash("IsOverPatrolRange");
        baseMaterial = GetComponent<SpriteRenderer>().material;
    }

    public IEnumerator Die(Action action)
    {
        Color current = GetComponent<SpriteRenderer>().color;
        while (current.a >= 0.3)
        {
            current.a -= 0.01f;
            GetComponent<SpriteRenderer>().color = current;
            yield return new WaitForSeconds(0.01f);
        }
        
        action.Invoke();
    }

    public void Damage(int damage)
    {
        StartCoroutine(DamageEffect());
        ChangeHp(-damage);
    }
    
    public IEnumerator DamageEffect()
    {
        GetComponent<SpriteRenderer>().color = new Color(1,0.7f,0.7f,1);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
    
    public virtual void PlayAttackAnimation(NormalMonsterController.StateType prevStateType)
    {
        switch (prevStateType)
        {
            case NormalMonsterController.StateType.Move: case NormalMonsterController.StateType.Idle:
                animator.SetBool(_detectHash,true);
                break;
        }
    }
    
    public virtual void PlayMoveAnimation(NormalMonsterController.StateType prevStateType)
    {
        switch (prevStateType)
        {
            case NormalMonsterController.StateType.Idle:
                animator.SetBool(_patrolOverHash, false);
                break;
            case NormalMonsterController.StateType.Attack:
                animator.SetBool(_detectHash,false);
                break;
        }
    }

    public virtual void PlayIdleAnimation(NormalMonsterController.StateType prevStateType)
    {
        switch (prevStateType)
        {
            case NormalMonsterController.StateType.Move:
                animator.SetBool(_patrolOverHash, true);
                break;
        }
        
    }

    public void InitHp(int hp)
    {
        this.hp.text = hp.ToString();
    }

    public void ChangeHp(int value)
    {
        var currentHp = int.Parse(hp.text);
        hp.text = (currentHp + value).ToString();
    }

    public void ChangeWarningMark(bool value)
    {
        warningMark.enabled = value;
    }

    public void TurnOnOutline()
    {
        GetComponent<SpriteRenderer>().material = outlineMaterial;
    }

    public void TurnOffOutline()
    {
        GetComponent<SpriteRenderer>().material = baseMaterial;
    }
}
