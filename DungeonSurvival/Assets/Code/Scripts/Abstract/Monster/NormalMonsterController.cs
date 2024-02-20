using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class NormalMonsterController : MonoBehaviour, INormalMonsterController, IDetectable, IDamageable, IChasable, ICollisionable, IPatrolable, IDropable, IDieable, IIdleable 
{
    #region Events
    
    public UnityAction<int> OnDamage;
    public UnityAction OnDie;
    
    #endregion

    #region Inspector fields
    
    [Header("초기 몬스터 정보")]
    public MonsterSO monsterSO;
    
    [Space(10)]
    [Header("인게임 일반 몬스터 공통 정보")]
    [Tooltip("게임 실행 시 실제로 적용되는 정보들로 MonsterSO로 초기 값을 세팅하고 실시간으로 값을 변경할 수 있다. ex) Hp 감소, 이동속도 증가")]
    [SerializeField] protected NormalMosterModel _model;

    [Space(10)]
    [ReadOnly]
    [Header("누적 추격 시간")]
    [Tooltip("플레이어를 발견하거나 데미지를 입을 때 유저를 추격하는데 그 시간의 합이다. monterSO에서 설정한 chaseTime을 넘어가면 추격이 중단된다.")]
    [SerializeField]
    protected float _accumulationChaseTime;
    
    #endregion

    #region Fields
    
    protected Transform _player;
    protected NormalMonsterStateMachine _stateMachine;
    protected NormalMonsterLogic _logic;
    protected NormalMonsterVisual _visual;
    protected MoveStrategy _currentMoveStrategy;
    protected PatrolStrategy _patrolStrategy;
    protected ChaseStrategy _chaseStrategy;
    protected bool _isCollision;
    protected float _collisionDelay;
    protected float _idleTime;
    
    #endregion

    #region Enum
    
    public enum StateType
    {
        Move,
        Attack,
        Idle
    }
    
    #endregion

    #region Life Cycle
    
    protected virtual void Awake()
    {
        SetLogicAndVisual();
        _model = new NormalMosterModel(monsterSO);
        _player = GameObject.FindWithTag("Player").transform;
        _stateMachine = new NormalMonsterStateMachine(this);
        
        _patrolStrategy = new PatrolStrategy(transform, _model.MoveSpeed, _logic, _visual, _model.PatrolRange, _stateMachine);
        _chaseStrategy = new ChaseStrategy(transform, _player, _model.MoveSpeed, _logic, _visual, _stateMachine);
        
        _collisionDelay = 1f;
        _idleTime = 2f;
        _accumulationChaseTime = _model.ChaseTime;
        
        OnDamage += Damage;
        OnDie += Die;
        _patrolStrategy.OnOverPatrolRange += Idle;
        
    }

    private void Start()
    {
        _visual.InitHp(_model.Hp);
        _stateMachine.Init(_stateMachine.NormalMonsterMoveState);
        SetMoveStrategy(_patrolStrategy);
    }

    protected virtual void Update()
    {
        _stateMachine.Update();
    }

    protected virtual void OnDestroy()
    {
        OnDamage -= Damage;
        OnDie -= Die;
        _patrolStrategy.OnOverPatrolRange -= Idle;
    }
    
    protected virtual void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        GetComponent<NormalMonsterDebug>().DrawDetectionCone(_player,_model.PatrolFov,_model.ViewDistance);
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        Collision(collision);
    }
    
    #endregion

    #region Functions
    
    private void SetLogicAndVisual()
    {
        _logic = GetComponentInChildren<NormalMonsterLogic>();
        _visual = GetComponentInChildren<NormalMonsterVisual>();
    }
    
    private void Alert()
    {
        _accumulationChaseTime = 0;
    }

    protected void Move()
    {
        _currentMoveStrategy.Move();
    }
    
    protected virtual void SetMoveStrategy(MoveStrategy moveStrategy)
    {
        _currentMoveStrategy = moveStrategy;
    }
    
    public void Idle()
    {
        _stateMachine.TransitionTo(_stateMachine.NormalMonsterIdleState);
    }
    
    public void PlayAttackAnimation(StateType prevStateType)
    {
        _visual.PlayAttackAnimation(prevStateType);
    }
    
    public void PlayMoveAnimation(StateType prevStateType)
    {
        _visual.PlayMoveAnimation(prevStateType);
    }

    public void PlayIdleAnimation(StateType prevStateType)
    {
        _visual.PlayIdleAnimation(prevStateType);
    }
    
    #endregion

    #region Virtual Functions
    
    public virtual void Detect()
    {
        bool isDetect = _logic.IsTargetDetected(_player, transform.GetChild(0).localScale.x, _model.ViewDistance,
            _model.PatrolFov);
        
        if (isDetect || _accumulationChaseTime < _model.ChaseTime)
        {
            if (isDetect)
            {
                Alert();
            }
            
            _stateMachine.TransitionTo(_stateMachine.NormalMonsterAttackState);
            return;
        }

        if (_stateMachine.CurrentState == _stateMachine.NormalMonsterIdleState)
        {
            return;
        }
        
        _stateMachine.TransitionTo(_stateMachine.NormalMonsterMoveState);
    }
    
    public virtual void Damage(int damage)
    {
        Alert();
        _visual.Damage(damage);
        _model.Hp = _logic.Damage(_model.Hp,damage);
        if (_model.Hp <= 0)
        {
            _stateMachine.TransitionTo(_stateMachine.NormalMonsterDieState);
        }
    }
    
    public virtual void Chase()
    {
        SetMoveStrategy(_chaseStrategy);
        _logic.ChangeVisualDirectionTowardTarget(transform.GetChild(0), _player, 1);
        _accumulationChaseTime += Time.deltaTime;
        Move();
    }
    
    public virtual void PlayCollisionSound(Vector3 targetPos)
    {
        SoundManager.Instance.PlaySfxAt(targetPos,monsterSO.AttackSound,false);
    }
    
    public virtual void Collision(Collision2D col)
    {
        if (col.transform.CompareTag("Player") && !PlayerController._isDamaged && !_isCollision)
        {
            StartCoroutine(CollsionDelay());
            PlayCollisionSound(col.transform.position);
            PlayerController.OnDamage.Invoke(monsterSO.Atk, transform);
        }
    }
    
    public virtual void Patrol()
    {
        SetMoveStrategy(_patrolStrategy);
        Move();
    }
    
    public virtual void Drop()
    {
        var dropItem = (ItemSO)monsterSO.DropItem;
        Instantiate(dropItem.VisualWorldPrefab, transform.position, transform.rotation);
    }
    
    public virtual void PlayDieAnimation()
    {
        StartCoroutine(_visual.Die(() =>
        {
            Drop();
            Destroy(gameObject);
        }));
    }
    
    public virtual void PlayDieSound()
    {
        SoundManager.Instance.PlaySfxAt(transform.position,monsterSO.DeadSound,false);
    }
    
    public virtual void Die()
    {
        PlayDieSound();
        PlayDieAnimation();
    }
    
    public virtual void PlayDetectSound()
    {
        SoundManager.Instance.PlayCommonMonsterSfxAt(transform.position,"Detect",false);
    }
    
    #endregion

    #region Coroutine

    IEnumerator CollsionDelay()
    {
        _isCollision = true;
        yield return new WaitForSeconds(_collisionDelay);
        _isCollision = false;
    }
    
    protected IEnumerator StartIdleTimer()
    {
        yield return new WaitForSeconds(_idleTime);
        if (_stateMachine.CurrentState == _stateMachine.NormalMonsterIdleState)
        {
            _stateMachine.TransitionTo(_stateMachine.NormalMonsterMoveState);
        }
    }

    #endregion
    
    #region StateVirtualFunc

    public virtual void EnterMoveState()
    {
        
    }

    public virtual void UpdateMoveState()
    {
        
    }

    public virtual void ExitMoveState()
    {
        
    }

    public virtual void EnterDieState()
    {
        
    }

    public virtual void UpdateDieState()
    {
        
    }

    public virtual void ExitDieState()
    {
        
    }

    public virtual void EnterIdleState()
    {
        
    }

    public virtual void UpdateIdleState()
    {
        
    }

    public virtual void ExitIdleState()
    {
        
    }

    public virtual void EnterAttackState()
    {
        
    }

    public virtual void UpdateAttackState()
    {
        
    }

    public virtual void ExitAttackState()
    {
        
    }
    
    #endregion
}
