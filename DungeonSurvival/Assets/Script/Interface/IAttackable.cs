using UnityEngine;

public interface IAttackable
{
    public void Attack(Collision2D col);
    public void PlayAttackAnimation();
}
