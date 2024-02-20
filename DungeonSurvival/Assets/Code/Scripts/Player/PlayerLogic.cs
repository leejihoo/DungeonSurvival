using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private PlayerModel _playerModel;

    private void Start()
    {
        _playerModel = gameObject.GetComponentInParent<PlayerController>().playerModel;
    }

    public void OnDamage(int value, Transform attacker)
    {
        Debug.Log("데미지: " + value);
        var knockbackDirection = (transform.position - attacker.position).normalized; 
        Knockback(knockbackDirection);
        var damagedHp = _playerModel.Hp - value;
        Debug.Log("남은 체력: " + damagedHp);
        if (damagedHp > 0)
        {
            _playerModel.Hp = damagedHp;
            return;
        }

        _playerModel.Hp = 0;
        PlayerController.OnDie.Invoke();
        // if (!PlayerController._isDamaged)
        // {
        //     
        // }

        StartCoroutine(UnDamage());
    }

    public void Knockback(Vector3 direction)
    {
        Debug.Log(direction);
        gameObject.GetComponentInParent<Rigidbody2D>().AddRelativeForce(new Vector2(direction.x * 5,direction.y * 5),ForceMode2D.Impulse);
        
        StartCoroutine(ExitKnockback());

    }

    IEnumerator ExitKnockback()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
    }
    
    // public void StartUnDamage(int value, Transform attacker)
    // {
    //     StartCoroutine(UnDamage());
    // }

    IEnumerator UnDamage()
    {
        PlayerController._isDamaged = true;
        yield return new WaitForSeconds(1f);
        PlayerController._isDamaged = false;
    }
    
}
