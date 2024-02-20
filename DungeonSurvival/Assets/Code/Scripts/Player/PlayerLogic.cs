using System.Collections;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    #region Fields
    
    private PlayerModel _playerModel;

    #endregion

    #region Life Cycle
    
    private void Start()
    {
        _playerModel = gameObject.GetComponentInParent<PlayerController>().playerModel;
    }

    #endregion

    #region Functions
    
    public void OnDamage(int value, Transform attacker)
    {
        var knockbackDirection = (transform.position - attacker.position).normalized; 
        Knockback(knockbackDirection);
        
        var damagedHp = _playerModel.Hp - value;
        if (damagedHp > 0)
        {
            _playerModel.Hp = damagedHp;
            return;
        }

        _playerModel.Hp = 0;
        PlayerController.OnDie.Invoke();

        StartCoroutine(UnDamage());
    }

    public void Knockback(Vector3 direction)
    {
        gameObject.GetComponentInParent<Rigidbody2D>().AddRelativeForce(new Vector2(direction.x * 5,direction.y * 5),ForceMode2D.Impulse);
        
        StartCoroutine(ExitKnockback());
    }

    #endregion

    #region Coroutine
    
    IEnumerator ExitKnockback()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
    }
    
    IEnumerator UnDamage()
    {
        PlayerController.IsDamaged = true;
        yield return new WaitForSeconds(1f);
        PlayerController.IsDamaged = false;
    }
    
    #endregion
}
