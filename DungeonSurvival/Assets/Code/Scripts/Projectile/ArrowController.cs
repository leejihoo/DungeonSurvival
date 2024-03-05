using UnityEngine;

public class ArrowController : ProjectileController
{
    public Transform target;

    protected override void Update()
    {
        SetDirection();
        transform.Translate(_direction * Time.deltaTime * speed, Space.World);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            return;
        }
        
        if (other.gameObject.CompareTag("Monster"))
        {
            PlayAttackSound(other.transform);
            Debug.Log("atk:" + atk);
            other.GetComponentInParent<NormalMonsterController>().OnDamage.Invoke(atk);
        }

        Destroy(gameObject);
    }
    
    protected override void SetDirection()
    {
        if (target != null)
        {
            _direction = (target.position - transform.position).normalized;
        }
    }
    
    public override void RotateProjectileTowardsTarget()
    {
        float angle = Vector3.SignedAngle(transform.up, _direction, Vector3.forward);
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        
        transform.rotation = targetRotation;
    }
}
