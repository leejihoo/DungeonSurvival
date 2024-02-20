using DG.Tweening;
using UnityEngine;

public class RushTypeMonsterVisual : NormalMonsterVisual
{
    private int _rushSpeedHash; 
    private float _maxSpeed;
    private float initialSpeed;
    public AnimationCurve rushSpeedAnimationCurve;
    
    protected override void Awake()
    {
        base.Awake();
        _rushSpeedHash = Animator.StringToHash("RushSpeed");
        initialSpeed = animator.GetFloat(_rushSpeedHash);
        _maxSpeed = 3f;
    }

    public void RushAnimationSpeedUp(float duration)
    {
        DOTween.To(() => initialSpeed, x => animator.SetFloat(_rushSpeedHash, x), _maxSpeed, duration)
            .SetEase(rushSpeedAnimationCurve).OnComplete(ResetRushAnimationSpeed);
    }

    public void ResetRushAnimationSpeed()
    {
        animator.SetFloat(_rushSpeedHash,initialSpeed);
    }
}
