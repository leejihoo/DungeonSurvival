using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTypeMonsterController : NormalMonsterController
{
    [Header("투척형 몬스터 추가 정보")]
    public GameObject projectile;
    [SerializeField] private Vector3 projectileInitPosChange;
    [Tooltip("다음 투척까지 필요한 시간이다. (default: 3초)")]
    [SerializeField] private float shootDelay;
    [Tooltip("투척을 한 뒤 측정되는 시간의 합이다. rushDelay보다 커지면 몬스터가 돌진을 하고 0으로 바뀐다.")]
    [SerializeField, ReadOnly] private float accumulationShootTime;
    [SerializeField, ReadOnly] private bool isAttackAnimationEnd;
    
    // exit가 작동되는 동안에도 update는 계속 작동되고 있어서 exit에서 바꾼 값이 update로 다시 바뀌는 현상을 방지하기 위한 변수
    private bool _isAttackStateExit;

    protected override void Awake()
    {
        base.Awake();
        if (shootDelay == 0f)
        {
            shootDelay = 3f;    
        }
        
        accumulationShootTime = shootDelay;
        ((ThrowTypeMonsterVisual)_visual).OnchargeRaise += OnCharge;
        
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (!Application.isPlaying) return;
        GetComponent<NormalMonsterDebug>().DrawAttackRange(_model.AttackRange);
    }

    public void OnCharge()
    {
        var instant = Instantiate(projectile, transform.position - projectileInitPosChange, transform.rotation);
        //_logic.ChangeVisualDirectionTowardTarget(instant.transform, _player, -1);
        // instant.GetComponent<ProjectileController>().atk = 3;
        // instant.GetComponent<ProjectileController>().speed = 3;
    }
    
    IEnumerator StartAttackAnimation()
    {
        isAttackAnimationEnd = false;
        yield return new WaitForSeconds(((ThrowTypeMonsterVisual)_visual).AttackAnimationTime);
        isAttackAnimationEnd = true;
    }

    #region StateFunc
    
    public override void EnterMoveState()
    {
        _visual.ChangeWarningMark(false);
    }

    public override void UpdateMoveState()
    {
        Patrol();
        Detect();
    }

    public override void ExitMoveState()
    {
        if (_stateMachine.NextState == _stateMachine.NormalMonsterIdleState)
        {
            PlayIdleAnimation(StateType.Move);
            return;
        }
        
        PlayAttackAnimation(StateType.Move);
    }

    public override void EnterDieState()
    {
        OnDie.Invoke();
    }

    public override void EnterAttackState()
    {
        _isAttackStateExit = false;
        _visual.ChangeWarningMark(true);
        PlayDetectSound();
    }

    public override void UpdateAttackState()
    {
        accumulationShootTime += Time.deltaTime;
        
        var isInAttackRange = ((ThrowTypeMonsterLogic)_logic).IsInAttackRange(_model.AttackRange, _player);
        if (!_isAttackStateExit)
        {
            if (!isInAttackRange)
            {
                PlayMoveAnimation(StateType.Attack);
                Chase();
            }
            else
            {
                if (accumulationShootTime >= shootDelay)
                {
                    StartCoroutine(StartAttackAnimation());
                    PlayAttackAnimation(StateType.Move);
                    accumulationShootTime = 0;
                }
                else
                {
                    if (isAttackAnimationEnd)
                    {
                        PlayMoveAnimation(StateType.Attack);
                        Chase();
                    }
                }
                
                _accumulationChaseTime += Time.deltaTime;
            }
        }
        
        Detect();
    }

    public override void ExitAttackState()
    {
        _isAttackStateExit = true;
        accumulationShootTime = shootDelay;
        PlayMoveAnimation(StateType.Attack);
    }

    public override void EnterIdleState()
    {
        StartCoroutine(StartIdleTimer());    
    }

    public override void UpdateIdleState()
    {
        Detect();
    }

    public override void ExitIdleState()
    {
        if (_stateMachine.NextState == _stateMachine.NormalMonsterMoveState)
        {
            PlayMoveAnimation(StateType.Idle);
            return;
        }
        
        PlayAttackAnimation(StateType.Idle);
    }
    
    #endregion
}
