using UnityEngine;

public interface ICollisionable : IAttackAnimationPlayable, ICollisionSoundPlayable
{
    public void Collision(Collision2D col);
}
