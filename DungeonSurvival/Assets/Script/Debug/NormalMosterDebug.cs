using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonsterDebug : MonoBehaviour
{
    public int tempdamage;
    public void DrawDetectionCone(Transform player, float detectionAngle, float detectionRadius)
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
