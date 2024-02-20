using System;
using System.Collections;
using System.Collections.Generic;
using Script.PluggableProgrammingTest;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PluggableMonsterController : MonoBehaviour, INormalMonsterController
{
    public UnityAction<int> OnDamage;
    private NormalMonsterStateMachine _stateMachine;
    private Transform _player;
    
    [SerializeField] private MonsterSO monsterSO;
    [SerializeField] private NormalMosterModel model;
    [SerializeField] private MoveListSO moveListSO;
    [SerializeField] private AttackListSO attackListSO;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private Image warningMark;
    
    private MoveSO _currentMoveSO;
    private AttackSO _currentAttackSO;
    
    void Awake()
    {
        model = new NormalMosterModel(monsterSO);
        _player = GameObject.FindWithTag("Player").transform;
        _stateMachine = new NormalMonsterStateMachine(this);
        SetMoveChaseInfo();
        SetMovePatrolInfo();
        SetAttackCollisionInfo();
        SetAttackSO(attackListSO.attackList[0]);
        OnDamage += Damage;
    }

    private void OnDestroy()
    {
        OnDamage -= Damage;
    }

    public void SetMoveChaseInfo()
    {
        ScriptableObject.CreateInstance<MoveChaseSO>();
        var temp = (MoveChaseSO)moveListSO.moveList[0];
        temp.target = _player;
        temp.mover = transform;
        temp.moveSpeed = model.MoveSpeed;
    }
    
    public void SetMovePatrolInfo()
    {
        var temp = (MovePatrolSO)moveListSO.moveList[1];
        temp.mover = transform;
        temp.moveSpeed = model.MoveSpeed;
        temp.moveDirection = 1;
        temp.movementChange = 0;
        temp.patrolRange = 5;
    }

    public void SetAttackCollisionInfo()
    {
        var temp = (AttackCollisionSO)attackListSO.attackList[0];
        temp.attackDamage = model.AttackRange;
        temp.attackRange = model.AttackRange;
        temp.attacker = transform;
        temp.target = _player;
        temp.attackSound = monsterSO.AttackSound;
    }
    
    void Start()
    {
        _stateMachine.Init(_stateMachine.NormalMonsterMoveState);
    }
    
    protected virtual void Update()
    {
        _stateMachine.Update();
    }
    
    protected virtual void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        GetComponent<PluggableDebug>().DrawDetectionCone(_player,model.PatrolFov,model.PatrolRange);
    }

    public void Attack()
    {
        _currentAttackSO.Attack();
    }

    public void SetAttackSO(AttackSO nextAttackSO)
    {
        _currentAttackSO = nextAttackSO;
    }

    public void Move()
    {
        _currentMoveSO.Move();
    }

    public void SetMoveSO(MoveSO nextMoveSO)
    {
        _currentMoveSO = nextMoveSO;
    }
    
    public void Detect()
    {
        bool isDetect = PluggableMonsterLogic.IsTargetDetected(_player,transform , transform.GetChild(0).localScale.x, model.ViewDistance,
            model.PatrolFov);
        
        if (isDetect)
        {
            _stateMachine.TransitionTo(_stateMachine.NormalMonsterAttackState);
            return;
        }
        _stateMachine.TransitionTo(_stateMachine.NormalMonsterMoveState);
        
    }
    
    public void Damage(int damage)
    {
        StartCoroutine(PluggableMonsterVisual.DamageEffect(transform.GetComponentInChildren<SpriteRenderer>()));
        PluggableMonsterVisual.ChangeHp(-damage, hpText);
        
        //     StartCoroutine(DamageEffect(target));
        //     ChangeHp(-damage, hpText);
        model.Hp = PluggableMonsterLogic.Damage(model.Hp,damage);
        if (model.Hp <= 0)
        {
            _stateMachine.TransitionTo(_stateMachine.NormalMonsterDieState);
        }
    }
    
    public void PlayDetectSound()
    {
        SoundManager.Instance.PlayCommonMonsterSfxAt(transform.position,"Detect",false);
    }
    
    public void Drop()
    {
        Debug.Log("생성됨");
        var dropitemSO = (ItemSO)monsterSO.DropItem;
        Instantiate(dropitemSO.VisualWorldPrefab, transform.position, transform.rotation);
    }

    public void Die()
    {
        PlayDieSound();
        PlayDieAnimation();
    }

    public void PlayDieSound()
    {
        SoundManager.Instance.PlaySfxAt(transform.position,monsterSO.DeadSound,false);
    }
    
    public void PlayDieAnimation()
    {
        StartCoroutine(PluggableMonsterVisual.Die(() =>
        {
            Drop();
            Destroy(gameObject);
        },transform.GetComponentInChildren<SpriteRenderer>()));
    }

    public void EnterMoveState()
    {
        SetMoveSO(moveListSO.moveList[1]);
        PluggableMonsterVisual.SetActiveImage(false, warningMark);
        PluggableMonsterVisual.PlayAnimation(transform.GetComponentInChildren<Animator>(),"IsDetectPlayer", false);
    }

    public void UpdateMoveState()
    {
        Move();
        Detect();
    }

    public void ExitMoveState()
    {
        
    }

    public void EnterDieState()
    {
        Die();
    }

    public void UpdateDieState()
    {
        
    }

    public void ExitDieState()
    {
        
    }

    public void EnterIdleState()
    {
        
    }

    public void UpdateIdleState()
    {
        
    }

    public void ExitIdleState()
    {
        
    }

    public void EnterAttackState()
    {
        PlayDetectSound();
        SetMoveSO(moveListSO.moveList[0]);
        PluggableMonsterVisual.SetActiveImage(true, warningMark);
        PluggableMonsterVisual.PlayAnimation(transform.GetComponentInChildren<Animator>(),"IsDetectPlayer", true);
    }

    public void UpdateAttackState()
    {
        Move();
        Attack();
        Detect();
    }

    public void ExitAttackState()
    {
        
    }
}
