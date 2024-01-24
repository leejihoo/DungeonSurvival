using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class NormalMosterController : MonoBehaviour, IController
{
    public static UnityAction<int> OnDamage;
    public static UnityAction OnDie;
    
    public NormalMosterModel model;

    [SerializeField] private MosterSO mosterSO;
    
    private Transform _player;
    private bool _isDetectPlayer;
    private StateMachine _stateMachine;
    private NormalMosterLogic _logic;
    private NormalMosterVisual _visual;
    private int _detectHash;
    private float _movementChange;
    private float _moveDirection;
    
    [SerializeField] private GameObject _normalMonsterDebug;
    private void Awake()
    {
        model = new NormalMosterModel(mosterSO);
        _player = GameObject.FindWithTag("Player").transform;

        _moveDirection = 1;
        _logic = GetComponentInChildren<NormalMosterLogic>();
        _visual = GetComponentInChildren<NormalMosterVisual>();
        _stateMachine = new StateMachine(this);

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
        _stateMachine.Init(_stateMachine.moveState);
        _visual.InitHp(model.Hp);
    }
    
    void Update()
    {
        _stateMachine.Update();
    }
    
    private void OnDrawGizmos()
    {
        _normalMonsterDebug.GetComponent<NormalMonsterDebug>().DrawDetectionCone(_player,model.PatrolFov,model.PatrolRange);
    }

    public void Detect()
    {
        bool isDetect = _logic.IsTargetDetected(_player, transform.GetChild(0).localScale.x, model.ViewDistance,
            model.PatrolFov);
        
        if (isDetect)
        {
            _stateMachine.TransitionTo(_stateMachine.attackState);
            return;
        }
        _stateMachine.TransitionTo(_stateMachine.moveState);
        
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
            _stateMachine.TransitionTo(_stateMachine.deadState);
        }
    }

    public void Chase()
    {
        transform.position = _logic.MoveToTarget(_player, mosterSO.MoveSpeed);
    }

    public void Attack(Collision2D col)
    {
        if (col.transform.CompareTag("Player") && !PlayerController._isDamaged)
        {
            PlayAttackSound(col.transform.position);
            Debug.Log("데미지 받는중");
            PlayerController.OnDamage.Invoke(mosterSO.Atk, transform);
        }
    }

    public void PlayAttackSound(Vector3 targetPos)
    {
        SoundManager.Instance.PlaySfxAt(targetPos,mosterSO.AttackSound,false);
    }

    public void Patrol()
    {
        transform.Translate(_logic.CalculatePatrolDistance(ref _movementChange, ref _moveDirection, mosterSO.MoveSpeed, model.PatrolRange, transform.GetChild(0)));
    }

    public void Drop()
    {
        Debug.Log("생성됨");
        var dropitemSO = (ItemSO)mosterSO.DropItem;
        Instantiate(dropitemSO.VisualWorldPrefab, transform.position, transform.rotation);
    }

    public void Die()
    {
        PlayDieSound();
        StartCoroutine(_visual.Die(() =>
        {
            Drop();
            Destroy(gameObject);
        }));
    }

    public void PlayDieSound()
    {
        SoundManager.Instance.PlaySfxAt(transform.position,mosterSO.DeadSound,false);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Attack(col);
    }
}
