    using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CyborgKangarooCyborgKangarooController : MonoBehaviour, ICyborgKangarooController
{
    public static UnityAction<int> OnDamage;
    public static UnityAction OnDie;
    
    public NormalMosterModel model;

    [SerializeField] private MonsterSO monsterSo;
    
    private Transform _player;
    private CyborgKangarooStateMachine cyborgKangarooStateMachine;
    private CyborgKangarooLogic _logic;
    private CyborgKangarooVisual _visual;
    private float _movementChange;
    private float _moveDirection;
    
    [SerializeField] private GameObject _normalMonsterDebug;
    private void Awake()
    {
        model = new NormalMosterModel(monsterSo);
        _player = GameObject.FindWithTag("Player").transform;

        _moveDirection = 1;
        _logic = GetComponentInChildren<CyborgKangarooLogic>();
        _visual = GetComponentInChildren<CyborgKangarooVisual>();
        cyborgKangarooStateMachine = new CyborgKangarooStateMachine(this);

        OnDamage += Damage;
        OnDie += Die;
    }

    private void OnDestroy()
    {
        OnDamage -= Damage;
        OnDie -= Die;
    }
    
    void Start()
    {
        cyborgKangarooStateMachine.Init(cyborgKangarooStateMachine.CyborgKangarooMoveState);
        _visual.InitHp(model.Hp);
    }
    
    void Update()
    {
        cyborgKangarooStateMachine.Update();
    }
    
    private void OnDrawGizmos()
    {
        _normalMonsterDebug.GetComponent<CyborgKangarooDebug>().DrawDetectionCone(_player,model.PatrolFov,model.PatrolRange);
    }

    public void Detect()
    {
        bool isDetect = _logic.IsTargetDetected(_player, transform.GetChild(0).localScale.x, model.ViewDistance,
            model.PatrolFov);
        
        if (isDetect)
        {
            cyborgKangarooStateMachine.TransitionTo(cyborgKangarooStateMachine.CyborgKangarooAttackState);
            return;
        }
        cyborgKangarooStateMachine.TransitionTo(cyborgKangarooStateMachine.CyborgKangarooMoveState);
        
    }

    public void PlayDetectSound()
    {
        SoundManager.Instance.PlayCommonMonsterSfxAt(transform.position,"Detect",false);
    }

    public void PlayAttackAnimation()
    {
        PlayDetectSound();
        _visual.PlayAttackAnimation();
    }
    
    public void PlayMoveAnimation()
    {
        _visual.PlayMoveAnimation();
    }
    
    // event로 관리
    public void Damage(int damage)
    {
        _visual.Damage(damage);
        model.Hp = _logic.Damage(model.Hp,damage);
        if (model.Hp <= 0)
        {
            cyborgKangarooStateMachine.TransitionTo(cyborgKangarooStateMachine.CyborgKangarooDieState);
        }
    }

    public void Chase()
    {
        transform.position = _logic.MoveToTarget(_player, monsterSo.MoveSpeed);
    }

    public void Collision(Collision2D col)
    {
        if (col.transform.CompareTag("Player") && !PlayerController.IsDamaged)
        {
            PlayCollisionSound(col.transform.position);
            Debug.Log("데미지 받는중");
            PlayerController.OnDamage.Invoke(monsterSo.Atk, transform);
        }
    }

    public void PlayCollisionSound(Vector3 targetPos)
    {
        SoundManager.Instance.PlaySfxAt(targetPos,monsterSo.AttackSound,false);
    }

    public void Patrol()
    {
        transform.Translate(_logic.CalculatePatrolDistance(ref _movementChange, ref _moveDirection, monsterSo.MoveSpeed, model.PatrolRange, transform.GetChild(0)));
    }

    public void Drop()
    {
        Debug.Log("생성됨");
        var dropitemSO = (ItemSO)monsterSo.DropItem;
        Instantiate(dropitemSO.VisualWorldPrefab, transform.position, transform.rotation);
    }

    public void Die()
    {
        PlayDieSound();
        PlayDieAnimation();
    }

    public void PlayDieSound()
    {
        SoundManager.Instance.PlaySfxAt(transform.position,monsterSo.DeadSound,false);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Collision(col);
    }

    public void PlayDieAnimation()
    {
        StartCoroutine(_visual.Die(() =>
        {
            Drop();
            Destroy(gameObject);
        }));
    }

    public void PlayAttackAnimation(NormalMonsterController.StateType prevStateType)
    {
        
    }

    public void PlayMoveAnimation(NormalMonsterController.StateType prevStateType)
    {
        
    }
}
