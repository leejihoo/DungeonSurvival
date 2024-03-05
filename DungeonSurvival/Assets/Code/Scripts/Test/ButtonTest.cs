using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private InputActionAsset inputSystem;
    private InputAction skillAction;    
    [SerializeField] private Image holder;
    [SerializeField] private GameObject skillRange;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform player;
    private Vector2 direction;
    private void Awake()
    {
        skillAction = inputSystem.FindAction("GamePlay/Skill");
        skillAction.Enable();
    }

    private void OnDestroy()
    {
        skillAction.Disable();
    }

    private void Update()
    {
        direction = skillAction.ReadValue<Vector2>();
        if (direction != Vector2.zero)
        {
            var angle = Vector2.SignedAngle(direction, Vector2.up);
            skillRange.transform.rotation = Quaternion.Euler(0f, 0f, -angle);
        }
        else
        {
            skillRange.transform.rotation = Quaternion.Euler(0,0,0);
        }

        // var value = skillAction.ReadValue<Vector2>();
        // if (value != Vector2.zero) // 입력이 있을 때만 회전
        // {
        //     var angle = Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg; // 조이스틱 벡터를 각도로 변환
        //     skillRange.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        // }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //var images = gameObject.GetComponentsInParent<Image>();
        // foreach (var image in images)
        // {
        //     image.color = new Color(1f,1f,1f,1f);
        // }
        gameObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
        holder.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // var images = gameObject.GetComponentsInParent<Image>();
        // foreach (var image in images)
        // {
        //     image.color = new Color(1f,1f,1f,0f);
        // }
        if (direction != Vector2.zero)
        {
            var instant = Instantiate(arrow, player.position + new Vector3(0,1.5f,0), player.rotation);
            instant.GetComponent<ArrowController>()._direction = direction.normalized;
        }

        gameObject.GetComponent<Image>().color = new Color(1f,1f,1f,0f);
        holder.GetComponent<Image>().color = new Color(1f,1f,1f,0f);
        skillRange.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(direction != Vector2.zero)
            skillRange.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
    }
}
