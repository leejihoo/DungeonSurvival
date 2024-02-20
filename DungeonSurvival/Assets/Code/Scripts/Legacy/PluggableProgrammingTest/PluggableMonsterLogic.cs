using UnityEngine;

namespace Script.PluggableProgrammingTest
{
    public class PluggableMonsterLogic : MonoBehaviour
    {
        public static Vector3 VectorToTarget(Transform targetPos, Transform currentPos)
        {
            return targetPos.position - currentPos.position;
        }

        public static float AngleToTarget(Transform targetPos, Transform currentPos, float visualDirection)
        {
            Vector3 vectorToTarget = VectorToTarget(targetPos,currentPos);
        
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

        public static bool IsTargetDetected(Transform targetPos, Transform currentPos, float visualDirection, float detectionRadius, float detectionAngle)
        {
            Vector3 vectorToTarget = VectorToTarget(targetPos, currentPos);
            float angleToTarget = AngleToTarget(targetPos, currentPos, visualDirection);
            if (detectionRadius >= vectorToTarget.magnitude && detectionAngle >= angleToTarget)
            {
                return true;
            }

            return false;
        }

        public static Vector3 MoveToTarget(Transform targetPos , Transform currentPos, float speed)
        {
            return Vector3.MoveTowards(currentPos.position, targetPos.position, speed * Time.deltaTime);
        }

        public static Vector3 CalculateDistance(float speed, float direction)
        {
            return  speed * Time.deltaTime * direction * Vector3.left;
        }

        public static bool IsOverPatrolRange(ref float movementChange, float speed, float direction, float maxPatrolDistance)
        {
            Vector3 distance = CalculateDistance(speed, direction);
            movementChange += distance.magnitude;

            return maxPatrolDistance < movementChange;
        }

        public static Vector3 CalculatePatrolDistance(ref float movementChange, ref float direction, float speed,  float maxPatrolDistance, Transform visual)
        {
            var distance = CalculateDistance(speed, direction);
            var isOverPatrolRange = IsOverPatrolRange(ref movementChange, speed, direction, maxPatrolDistance);
        
            if (isOverPatrolRange)
            {
                Debug.Log("범위를 벗어났습니다.");
                visual.localScale = Vector3.Scale(visual.localScale,new Vector3(-1,1,1));
                direction *= -1;
                movementChange = 0;
                return distance * -1;
            }

            return distance;
        }

        public static int Damage(int hp, int damage)
        {
            return hp - damage;
        }
    }
}
