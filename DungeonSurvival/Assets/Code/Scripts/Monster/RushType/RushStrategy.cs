using System;
using UnityEngine;
using DG.Tweening;
public class RushStrategy : MoveStrategy
{
    protected Transform _target;
    public Action OnRushComplete;
    
    public RushStrategy(Transform mover, Transform target, int moveSpeed, NormalMonsterLogic logic, NormalMonsterVisual visual, StateMachine stateMachine) : base(mover, moveSpeed, logic, visual, stateMachine)
    {
        MoveType = "Rush";
        _target = target;
    }

    public override void Move()
    {
        float distance = Vector3.Distance(_mover.position, _target.position);
        float duration = distance / (_moveSpeed * 2f);
        ((RushTypeMonsterVisual)_visual).RushAnimationSpeedUp(duration);
        _mover.DOMove(_target.position, duration).SetEase(Ease.InOutQuad).OnComplete(OnRushComplete.Invoke).OnPause(OnRushComplete.Invoke);
    }
}
