using System;
using UnityEngine;

public abstract class NormalMonsterLogic : MonoBehaviour
{
    #region Functions
    
    public Vector3 VectorToTarget(Transform targetPos)
    {
        return targetPos.position - transform.position;
    }

    public float AngleToTarget(Transform targetPos, float visualDirection)
    {
        Vector3 vectorToTarget = VectorToTarget(targetPos);
        float angleToTarget;
        
        if (visualDirection > 0)
        {
            angleToTarget = Vector3.Angle(Vector3.left, vectorToTarget.normalized);
        }
        else
        {
            angleToTarget = Vector3.Angle(Vector3.right, vectorToTarget.normalized);
        }

        return angleToTarget;
    }

    public bool IsTargetDetected(Transform targetPos, float visualDirection, float detectionRadius, float detectionAngle)
    {
        Vector3 vectorToTarget = VectorToTarget(targetPos);
        float angleToTarget = AngleToTarget(targetPos, visualDirection);
        
        if (detectionRadius >= vectorToTarget.magnitude && detectionAngle >= angleToTarget)
        {
            return true;
        }

        return false;
    }

    public Vector3 MoveToTarget(Transform targetPos, float speed)
    {
        return Vector3.MoveTowards(transform.position, targetPos.position, speed * Time.deltaTime);
    }

    public Vector3 CalculateMovementChange(float speed, float direction)
    {
        return  speed * Time.deltaTime * direction * Vector3.left;
    }

    public bool CheckIsOverPatrolRange(ref float movementChange, float speed, float direction, float maxPatrolDistance)
    {
        Vector3 distance = CalculateMovementChange(speed, direction);
        movementChange += distance.magnitude;

        return maxPatrolDistance < movementChange;
    }

    public Vector3 CalculatePatrolMovementChange(ref float movementChange, ref float direction, float speed,  float maxPatrolDistance, Transform visual, Action onOverPatrolRange)
    {
        var distance = CalculateMovementChange(speed, direction);
        var isOverPatrolRange = CheckIsOverPatrolRange(ref movementChange, speed, direction, maxPatrolDistance);

        if (visual.localScale.x * direction < 0)
        {
            ChangeVisualDirection(visual);
        }
        
        if (isOverPatrolRange)
        {
            onOverPatrolRange.Invoke();
            direction *= -1;
            movementChange = 0;
            return distance * -1;
        }

        return distance;
    }

    public void ChangeVisualDirection(Transform visual)
    {
        visual.localScale = Vector3.Scale(visual.localScale,new Vector3(-1 ,1,1));
    }
    
    public void ChangeVisualDirectionTowardTarget(Transform visual, Transform target, int direction)
    {
        Vector3 vectorToTarget = VectorToTarget(target);
        
        // 몬스터가 왼쪽을 보는게 + 방향이다.
        if (vectorToTarget.x * visual.localScale.x * direction > 0)
        {
            ChangeVisualDirection(visual);
        }
    }

    public int Damage(int hp, int damage)
    {
        return hp - damage;
    }
    
    #endregion
}
