using UnityEngine;

public class NormalMonsterDebug : MonoBehaviour
{
    #region Fields
    
    // editor에서 접근해야되서 접근자를 public 설정. inspector에서 변경 불가능하도록 ReadOnly를 사용
    [ReadOnly] public NormalMonsterController controller;
    
    #endregion

    #region Inspector Fields
    
    [Space(10)]
    [Header("임시 데미지")]
    public int tempDamage;

    #endregion

    #region Life Cycle
    
    private void Awake()
    {
        controller = GetComponent<NormalMonsterController>();
    }

    #endregion

    #region Functions
    
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

        // 반투명 노란색
        Gizmos.color = new Color(1f, 1f, 0f, 0.5f);

        var position = transform.position;

        // 감지 부채꼴 그리기
        Gizmos.DrawLine(position, position + leftPoint * detectionRadius);
        Gizmos.DrawLine(position, position + rightPoint * detectionRadius);
        
        Gizmos.DrawWireSphere(position, detectionRadius);
        
    }

    public void DrawAttackRange(int attackRange)
    {
        var position = transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, attackRange);
    }
    
    #endregion
}
