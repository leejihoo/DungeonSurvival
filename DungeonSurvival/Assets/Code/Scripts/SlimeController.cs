using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlimeController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;

    [SerializeField] private float detectionRadius;
    [SerializeField] private float detectionAngle;

    private bool isDetectPlayer;
    private int detectHash;
    private float maxPatrolDistance;
    private float movementChange;
    private float moveDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        moveDirection = 1;
        detectionRadius = 5f;
        detectionAngle = 45f;
        maxPatrolDistance = 5f;
        detectHash = Animator.StringToHash("IsDetectPlayer");
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer;
        
        if (transform.GetChild(0).localScale.x > 0)
        {
            angleToPlayer = Vector3.Angle(Vector3.left, directionToPlayer.normalized);
        }
        else
        {
            angleToPlayer = Vector3.Angle(Vector3.right, directionToPlayer.normalized);
        }
        
        if (detectionRadius >= directionToPlayer.magnitude && detectionAngle >= angleToPlayer)
        {
            isDetectPlayer = true;
            animator.SetBool(detectHash,isDetectPlayer);
            Debug.Log("플레이어 감지!" + "거리: " + directionToPlayer.magnitude +", 각도: " + angleToPlayer);
            return;
        }

        isDetectPlayer = false;
        animator.SetBool(detectHash,isDetectPlayer);
    }

    private void FixedUpdate()
    {
        if (isDetectPlayer)
        {
            MoveToPlayer();
            return;
        }

        Patrol();
    }

    private void OnDrawGizmos()
    {
        DrawDetectionCone();
    }

    void MoveToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, 2f * Time.deltaTime);
    }

    void Patrol()
    {
        var distance = Vector3.left * 2f * Time.deltaTime * moveDirection;
        movementChange += distance.magnitude;
        if (movementChange >= maxPatrolDistance)
        {
            transform.GetChild(0).localScale = Vector3.Scale(transform.GetChild(0).localScale,new Vector3(-1,1,1));
            moveDirection *= -1;
            movementChange = 0;
            transform.Translate(distance * -1);
            return;
        }
        transform.Translate(distance);
    }
    
    void DrawDetectionCone()
    {
        if (player == null)
            return;
        
        // 몬스터의 정면 방향 벡터
        Vector3 forwardVector;
        
        if (transform.GetChild(0).localScale.x > 0)
        {
            forwardVector = Vector3.left;
        }
        else
        {
            forwardVector = Vector3.right;
        }
        
        // 감지 부채꼴의 왼쪽 끝점
        Vector3 leftPoint = Quaternion.Euler(0, 0, -detectionAngle) * forwardVector;

        // 감지 부채꼴의 오른쪽 끝점
        Vector3 rightPoint = Quaternion.Euler(0, 0, detectionAngle) * forwardVector;

        Gizmos.color = new Color(1f, 1f, 0f, 0.5f); // 반투명 노란색

        // 감지 부채꼴 그리기
        Gizmos.DrawLine(transform.position, transform.position + leftPoint * detectionRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightPoint * detectionRadius);
        
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
    }

}
