using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public abstract class NormalMonsterVisual : MonoBehaviour
{
    #region Fields
    
    protected Animator animator;
    protected Image warningMark;
    protected TMP_Text hp;
    protected int _detectHash;
    protected int _patrolOverHash;
    
    private Material baseMaterial;
    
    #endregion

    #region Inspector Fields

    [SerializeField] private Material outlineMaterial;
    
    #endregion

    #region Life cycle
    
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

    #endregion
    
    #region Functions
    
    public void PlayAttackAnimation(NormalMonsterController.StateType prevStateType)
    {
        switch (prevStateType)
        {
            case NormalMonsterController.StateType.Move: case NormalMonsterController.StateType.Idle:
                animator.SetBool(_detectHash,true);
                break;
        }
    }
    
    public void PlayMoveAnimation(NormalMonsterController.StateType prevStateType)
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

    public void PlayIdleAnimation(NormalMonsterController.StateType prevStateType)
    {
        switch (prevStateType)
        {
            case NormalMonsterController.StateType.Move:
                animator.SetBool(_patrolOverHash, true);
                break;
        }
    }
    
    public void Damage(int damage)
    {
        StartCoroutine(DamageEffect());
        ChangeHp(-damage);
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
    
    #endregion

    #region Coroutine
    
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
    
    public IEnumerator DamageEffect()
    {
        GetComponent<SpriteRenderer>().color = new Color(1,0.7f,0.7f,1);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
    
    #endregion
}
