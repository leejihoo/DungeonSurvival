using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [Header("기본 정보")]
    public ProjectileSO projectileSO;
    
    [Space(10)]
    [Header("인게임 정보")]
    [ReadOnly] public int atk;
    [ReadOnly] public float speed;
    
    protected Vector3 _direction;
    protected virtual void Awake()
    {
        if (projectileSO != null)
        {
            atk = projectileSO.Atk;
            speed = projectileSO.Speed;
        }
    }

    protected virtual void Start()
    {
        // 자식인 arrowController에서 인스턴스화 직후에 target을 설정해줘서 awake에 있으면 nullReference 에러가 발생한다.
        SetDirection();
        RotateProjectileTowardsTarget();
    }

    protected virtual void SetDirection()
    {
        var targetPos = GameObject.FindWithTag("Player").transform.position;
        _direction = (targetPos - transform.position).normalized;
    }

    protected virtual void Update()
    {
        transform.Translate(_direction * Time.deltaTime * speed, Space.World);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            return;
        }
        
        if (other.gameObject.CompareTag("Player"))
        {
            PlayAttackSound(other.transform);
            PlayerController.OnDamage.Invoke(atk,transform);
        }

        Destroy(gameObject);
    }

    protected void PlayAttackSound(Transform target)
    {
        SoundManager.Instance.PlaySfxAt(target.position,projectileSO.AttackSound,false);
    }
    
    public virtual void RotateProjectileTowardsTarget()
    {
        float angle = Vector3.SignedAngle(transform.right, _direction, Vector3.forward);
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        
        transform.rotation = targetRotation;
    }
}
