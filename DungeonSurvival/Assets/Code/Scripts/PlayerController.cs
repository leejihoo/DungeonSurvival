using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.TouchPhase;

public class PlayerController : Singleton<PlayerController>
{
    public InputActionAsset InputAction;
    public InputActionAsset temp;
    [SerializeField] private float speed;
    [SerializeField] private InventoryHandler inventoryHandler;
    [SerializeField] private GameObject inventory;
    public PlayerModel playerModel;
    [SerializeField] private PlayerSO defaultPlayerSO;
    [SerializeField] private GameObject arrow;
    [SerializeField] private float attackDelay;
    [SerializeField] private GameObject bow;
    [SerializeField] private GameObject axe;
    [SerializeField] private GameObject cissor;
    
    private InputAction m_MoveAction;
    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody;
    private InputAction interact;
    private PlayerVisual _playerVisual;
    private PlayerLogic _playerLogic;
    private float prevX = 1.0f;
    private GameObject currentTarget;
    private bool _isEndAttackDelay;
    private GameObject _currentWearingTool;
    
    public static UnityAction<int, Transform> OnDamage;
    public static UnityAction OnDie;
    public static bool _isDamaged;
    
    // 나중에 sceneManager class를 만들어서 분리해줘야됨.
    [SerializeField] private Material coverMaterial;
    
    public override void Awake()
    {
        base.Awake();
        _playerVisual = gameObject.GetComponentInChildren<PlayerVisual>();
        _playerLogic = gameObject.GetComponentInChildren<PlayerLogic>();
        playerModel = new PlayerModel(defaultPlayerSO);
        m_MoveAction = InputAction.FindAction("Gameplay/Move");
        m_MoveAction.Enable();

        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponentInChildren<Animator>();
        _isEndAttackDelay = true;
    }

    // Start is called before the first frame update
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
    
    public void FadeIn(Scene scene, LoadSceneMode mode)
    {
        GetComponentInChildren<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        coverMaterial.DOFloat(1f, "_Progress", 2f);
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
                        m_Animator.SetTrigger("IsAttack");
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
                m_Animator.SetBool(collectHash,true);
            }
            else if (_currentWearingTool == axe)
            {
                var mineHash = Animator.StringToHash("IsMine");
                m_Animator.SetBool(mineHash,true);
            }

        }

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
        
        // foreach (var col in colliders)
        // {
        //     if (col.CompareTag("Monster"))
        //     {
        //         var distanceToCurrentTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
        //         var distanceToOtherTarget = Vector3.Distance(transform.position, col.transform.position);
        //     }
        // }
    }

    void FixedUpdate()
    {
        var move = m_MoveAction.ReadValue<Vector2>();
        var movement = move * speed;
        var moveHash = Animator.StringToHash("IsMove");
        var speedHash = Animator.StringToHash("Speed");
        var collectHash = Animator.StringToHash("IsCollect");
        var mineHash = Animator.StringToHash("IsMine");
        
        //note: == and != for vector2 is overriden to take in account floating point imprecision.
        if (move != Vector2.zero)
        {
            if (move.x * prevX < 0)
            {
                transform.GetChild(0).localScale = Vector3.Scale(transform.GetChild(0).localScale,new Vector3(-1,1,1));
                prevX = move.x;
            }

            m_Animator.SetBool(collectHash,false);
            m_Animator.SetBool(mineHash,false);
            m_Animator.SetBool(moveHash, true);
            //m_Rigidbody.MovePosition(m_Rigidbody.position + movement * Time.deltaTime);
            transform.Translate(movement * Time.deltaTime);
            return;
        }
        
        m_Animator.SetBool(moveHash, false);

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Item"))
        {
            inventoryHandler.UpdateItem(col.GetComponent<SOContainer>().item);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,10);
    }

    IEnumerator StartAttackDelay()
    {
        Debug.Log("공격딜레이 시작");
        _isEndAttackDelay = false;
        yield return new WaitForSeconds(attackDelay);
        _isEndAttackDelay = true;
        // if (_isEndAttackDelay)
        // {
        //     
        // }
        Debug.Log("공격딜레이 끝");
    }
}
