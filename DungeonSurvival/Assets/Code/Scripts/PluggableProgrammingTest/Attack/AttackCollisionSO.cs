using System.Collections;
using System.Collections.Generic;
using Script.PluggableProgrammingTest;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/CreateAttack/Collision", fileName = "AttackCollision")]
public class AttackCollisionSO : AttackSO
{
    public float attackRange;
    public int attackDamage;
    
    public override void Attack()
    {
        var distance = PluggableMonsterLogic.VectorToTarget(target, attacker).magnitude;
        if (distance <= attackRange)
        {
            PlayHitSound(target.transform.position);
            Debug.Log("충돌 데미지 받는중");
            PlayerController.OnDamage.Invoke(attackDamage, attacker);
        }
    }
}
