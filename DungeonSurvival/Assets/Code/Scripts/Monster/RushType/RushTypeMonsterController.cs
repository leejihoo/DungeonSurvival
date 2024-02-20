using DG.Tweening;
using UnityEngine;

public class RushTypeMonsterController : NormalMonsterController
{
    #region Inspector Fields
    
    [Header("돌진형 몬스터 추가 정보")]
    [Tooltip("다음 돌진까지 필요한 시간이다. (default: 3초)")]
    [SerializeField] private float rushDelay;
    [Tooltip("돌진을 한 뒤 측정되는 시간의 합이다. rushDelay보다 커지면 몬스터가 돌진을 하고 0으로 바뀐다.")]
    [SerializeField, ReadOnly] private float accumulationRushTime;
    
    #endregion

    #region Fields
    
    private RushStrategy _rushStrategy;
    // exit가 작동되는 동안에도 update는 계속 작동되고 있어서 exit에서 바꾼 값이 update로 다시 바뀌는 현상을 방지하기 위한 변수
    private bool _isAttackStateExit;
    private bool _isRushCompleted;
    
    #endregion

    #region Life Cycle
    
    protected override void Awake()
    {
        base.Awake();
        _rushStrategy = new RushStrategy(transform, _player, _model.MoveSpeed, _logic, _visual, _stateMachine);
        
        if (rushDelay == 0f)
        {
            rushDelay = 3f;    
        }
        
        accumulationRushTime = rushDelay;
        _isRushCompleted = true;
        _rushStrategy.OnRushComplete += RushComplete;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        transform.DOKill();
        _rushStrategy.OnRushComplete -= RushComplete;
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        // DoMove를 사용했을 때 collider를 무시하는 걸 방지하기 위한 코드
        transform.DOPause();
    }

    #endregion

    #region Functions

    public void Rush()
    {
        _isRushCompleted = false;
        //setMoveStrategy는 stateEnter할 때 한번만 해줘도 되는데 update동안 돌진 후 추적을 한다는 등 이동 전략이 바뀌면 문제가 생겨 각 이동 함수에 넣어둔다.
        SetMoveStrategy(_rushStrategy);
        _logic.ChangeVisualDirectionTowardTarget(transform.GetChild(0), _player, 1);
        Move();
    }

    public void RushComplete()
    {
        Debug.Log("러쉬 완료");
        _isRushCompleted = true;
    }

    #region MoveState
    
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

    #endregion

    #region DieState
    
    public override void EnterDieState()
    {
        OnDie.Invoke();
    }

    #endregion

    #region AttackState
    
    public override void EnterAttackState()
    {
        _isAttackStateExit = false;
        _visual.ChangeWarningMark(true);
        PlayDetectSound();
    }

    public override void UpdateAttackState()
    {
        Detect();
        accumulationRushTime += Time.deltaTime;
        _accumulationChaseTime += Time.deltaTime;
        
        if (accumulationRushTime >= rushDelay && !_isAttackStateExit && _isRushCompleted)
        {
            accumulationRushTime = 0;
            Rush();
        }
    }

    public override void ExitAttackState()
    {
        _isAttackStateExit = true;
        accumulationRushTime = rushDelay;
        transform.DOPause();
        PlayMoveAnimation(StateType.Attack);
    }

    #endregion

    #region IdleState
    
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
    
    #endregion
}
