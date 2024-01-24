using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.TouchPhase;

public class PlayerController : MonoBehaviour
{
    public InputActionAsset InputAction;
    public InputActionAsset temp;
    [SerializeField] private float speed;
    [SerializeField] private InventoryHandler inventoryHandler;
    [SerializeField] private GameObject inventory;
    public PlayerModel playerModel;
    [SerializeField] private PlayerSO defaultPlayerSO;
    
    private InputAction m_MoveAction;
    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody;
    private InputAction interact;
    private PlayerVisual _playerVisual;
    private PlayerLogic _playerLogic;
    private float prevX = 1.0f;
    
    
    public static UnityAction<int, Transform> OnDamage;
    public static UnityAction OnDie;
    public static bool _isDamaged;
    
    private void Awake()
    {
        _playerVisual = gameObject.GetComponentInChildren<PlayerVisual>();
        _playerLogic = gameObject.GetComponentInChildren<PlayerLogic>();
        playerModel = new PlayerModel(defaultPlayerSO);
        m_MoveAction = InputAction.FindAction("Gameplay/Move");
        m_MoveAction.Enable();

        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponentInChildren<Animator>();
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

    private void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            inventory.SetActive(!inventory.activeSelf);
        }
    }

    void FixedUpdate()
    {
        var move = m_MoveAction.ReadValue<Vector2>();
        var movement = move * speed;
        var moveHash = Animator.StringToHash("IsMove");
        var speedHash = Animator.StringToHash("Speed");
        
        //note: == and != for vector2 is overriden to take in account floating point imprecision.
        if (move != Vector2.zero)
        {
            if (move.x * prevX < 0)
            {
                transform.GetChild(0).localScale = Vector3.Scale(transform.GetChild(0).localScale,new Vector3(-1,1,1));
                prevX = move.x;
            }

            
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

    
    
}
