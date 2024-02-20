using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyborgKangarooLogic : MonoBehaviour
{
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
            //Debug.Log("플레이어 감지!" + "거리: " + vectorToTarget.magnitude +", 각도: " + angleToTarget);
            return true;
        }

        return false;
    }

    public Vector3 MoveToTarget(Transform targetPos, float speed)
    {
        return Vector3.MoveTowards(transform.position, targetPos.position, speed * Time.deltaTime);
    }

    public Vector3 CalculateDistance(float speed, float direction)
    {
        return  speed * Time.deltaTime * direction * Vector3.left;
    }

    public bool IsOverPatrolRange(ref float movementChange, float speed, float direction, float maxPatrolDistance)
    {
        Vector3 distance = CalculateDistance(speed, direction);
        movementChange += distance.magnitude;

        return maxPatrolDistance < movementChange;
    }

    public Vector3 CalculatePatrolDistance(ref float movementChange, ref float direction, float speed,  float maxPatrolDistance, Transform visual)
    {
        var distance = CalculateDistance(speed, direction);
        var isOverPatrolRange = IsOverPatrolRange(ref movementChange, speed, direction, maxPatrolDistance);
        
        if (isOverPatrolRange)
        {
            visual.localScale = Vector3.Scale(visual.localScale,new Vector3(-1,1,1));
            direction *= -1;
            movementChange = 0;
            return distance * -1;
        }

        return distance;
    }

    public int Damage(int hp, int damage)
    {
        return hp - damage;
    }
}
