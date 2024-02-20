using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : Singleton<PlayerController>
{
    #region Inspector Fields
    
    public InputActionAsset inputAction;
    public PlayerModel playerModel;
    [SerializeField] private float speed;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private GameObject inventory;
    [SerializeField] private PlayerSO defaultPlayerSO;
    [SerializeField] private GameObject arrow;
    [SerializeField] private float attackDelay;
    [SerializeField] private GameObject bow;
    [SerializeField] private GameObject axe;
    [SerializeField] private GameObject cissor;
    
    // 나중에 sceneManager class를 만들어서 분리해줘야됨.
    [SerializeField] private Material coverMaterial;
    
    #endregion

    #region Fields
    
    private InputAction _moveAction;
    private Animator _animator;
    private InputAction interact;
    private PlayerVisual _playerVisual;
    private PlayerLogic _playerLogic;
    private GameObject currentTarget;
    private GameObject _currentWearingTool;
    private bool _isEndAttackDelay;
    private float prevX = 1.0f;
    
    public static bool IsDamaged;
    
    #endregion

    #region Events
    
    public static UnityAction<int, Transform> OnDamage;
    public static UnityAction OnDie;
    
    #endregion

    #region Life Cycle
    
    public override void Awake()
    {
        base.Awake();
        
        _playerVisual = gameObject.GetComponentInChildren<PlayerVisual>();
        _playerLogic = gameObject.GetComponentInChildren<PlayerLogic>();
        playerModel = new PlayerModel(defaultPlayerSO);
        _moveAction = inputAction.FindAction("Gameplay/Move");
        _moveAction.Enable();
        
        _animator = GetComponentInChildren<Animator>();
        _isEndAttackDelay = true;
        
        inventoryController.inventoryData = new Dictionary<string, InventoryItemData>();
        inventoryController.isSlotEmpty = new bool[inventoryController.maxInventorySize];
        Array.Fill(inventoryController.isSlotEmpty,true);
    }
    
    void Start()
    {
        OnDamage += _playerLogic.OnDamage;
        OnDamage += _playerVisual.OnDamage;
        OnDie += _playerVisual.OnDie;
        _playerVisual.InitHp(5);
    }

    private void OnDestroy()
    {
        OnDamage -= _playerLogic.OnDamage;
        OnDamage -= _playerVisual.OnDamage;
        OnDie -= _playerVisual.OnDie;
    }
    
    // 나중에 sceneManager class를 만들어서 분리해줘야됨.
    private void OnEnable()
    {
        SceneManager.sceneLoaded += FadeIn;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= FadeIn;
    }
    
    private void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            inventory.SetActive(!inventory.activeSelf);
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            if (_currentWearingTool != null)
            {
                _currentWearingTool.SetActive(false);
                _currentWearingTool = null;
            }
        }
        
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            if (_currentWearingTool != null)
            {
                _currentWearingTool.SetActive(false);
            }
            
            _currentWearingTool = bow;
            _currentWearingTool.SetActive(true);
        }
        
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            if (_currentWearingTool != null)
            {
                _currentWearingTool.SetActive(false);
            }
            
            _currentWearingTool = cissor;
            _currentWearingTool.SetActive(true);
        }
        
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            if (_currentWearingTool != null)
            {
                _currentWearingTool.SetActive(false);
            }
            
            _currentWearingTool = axe;
            _currentWearingTool.SetActive(true);
        }
        
        if (Keyboard.current.ctrlKey.wasPressedThisFrame)
        {
            // 장비들을 객체화 한 다음 실제 실행되는 로직들은 장비들한테 넘겨주면 좋을듯
            if (_currentWearingTool == null)
            {
                Debug.Log("아무것도 착용하지 않았습니다.");    
            }
            else if (_currentWearingTool == bow)
            {
                if (currentTarget != null)
                {
                    if (_isEndAttackDelay)
                    {
                        _animator.SetTrigger("IsAttack");
                        var instant = Instantiate(arrow, transform.position + new Vector3(0,1.5f,0), transform.rotation);
                        instant.GetComponent<ArrowController>().target = currentTarget.transform;
                        StartCoroutine(StartAttackDelay());
                    }
                }
                else
                {
                    Debug.Log("대상이 없습니다..");
                }
            }
            else if (_currentWearingTool == cissor)
            {
                var collectHash = Animator.StringToHash("IsCollect");
                _animator.SetBool(collectHash,true);
            }
            else if (_currentWearingTool == axe)
            {
                var mineHash = Animator.StringToHash("IsMine");
                _animator.SetBool(mineHash,true);
            }
        }

        // 마물 자동 타겟
        var colliders = Physics2D.OverlapCircleAll(transform.position, 10);
        if (colliders != null)
        {
            if (currentTarget != null)
            {
                var distanceToCurrentTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
        
        
                var minDistanceTarget = (from col in colliders
                    let distance = Vector3.Distance(transform.position, col.transform.position)
                    where distance < distanceToCurrentTarget && col.CompareTag("Monster")
                    orderby distance
                    select col).FirstOrDefault();
        
                if (!ReferenceEquals(minDistanceTarget, null))
                {
                    currentTarget.GetComponentInChildren<NormalMonsterVisual>().TurnOffOutline();
                    currentTarget = minDistanceTarget.gameObject;
                    currentTarget.GetComponentInChildren<NormalMonsterVisual>().TurnOnOutline();
                }
            }
            else
            {
                var minDistanceTarget = (from col in colliders
                    let distance = Vector3.Distance(transform.position, col.transform.position)
                    where col.CompareTag("Monster")
                    orderby distance
                    select col).FirstOrDefault();
            
                if (minDistanceTarget != null)
                {
                    currentTarget = minDistanceTarget.gameObject;
                    currentTarget.GetComponentInChildren<NormalMonsterVisual>().TurnOnOutline();
                }
            }
        }
        else
        {
            currentTarget.GetComponentInChildren<NormalMonsterVisual>().TurnOffOutline();
            currentTarget = null;
        }
    }

    void FixedUpdate()
    {
        var move = _moveAction.ReadValue<Vector2>();
        var movement = move * speed;
        var moveHash = Animator.StringToHash("IsMove");
        var collectHash = Animator.StringToHash("IsCollect");
        var mineHash = Animator.StringToHash("IsMine");
        
        if (move != Vector2.zero)
        {
            if (move.x * prevX < 0)
            {
                transform.GetChild(0).localScale = Vector3.Scale(transform.GetChild(0).localScale,new Vector3(-1,1,1));
                prevX = move.x;
            }

            _animator.SetBool(collectHash,false);
            _animator.SetBool(mineHash,false);
            _animator.SetBool(moveHash, true);
            transform.Translate(movement * Time.deltaTime);
            return;
        }
        
        _animator.SetBool(moveHash, false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Item"))
        {
            inventoryController.UpdateItem(col.GetComponent<SOContainer>().item);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,10);
    }
    
    #endregion

    #region Functions
    
    // 나중에 sceneManager로 분리해야 됨.
    public void FadeIn(Scene scene, LoadSceneMode mode)
    {
        GetComponentInChildren<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        coverMaterial.DOFloat(1f, "_Progress", 2f);
    }

    #endregion

    #region Coroutine
    
    IEnumerator StartAttackDelay()
    {
        _isEndAttackDelay = false;
        yield return new WaitForSeconds(attackDelay);
        _isEndAttackDelay = true;
    }
    
    #endregion
}
