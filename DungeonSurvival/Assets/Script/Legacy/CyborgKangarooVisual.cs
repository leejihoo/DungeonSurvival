using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CyborgKangarooVisual : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Image warningMark;
    [SerializeField] private TMP_Text hp; 
    private int _detectHash;
    
    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        _detectHash = Animator.StringToHash("IsDetectPlayer");
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
    
    public void PlayAttackAnimation()
    {
        ChangeWarningMark(true);
        animator.SetBool(_detectHash,true);
    }
    
    public void PlayMoveAnimation()
    {
        ChangeWarningMark(false);
        animator.SetBool(_detectHash,false);
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
        Debug.Log("마크변환작동");
        warningMark.enabled = value;
    }
}
