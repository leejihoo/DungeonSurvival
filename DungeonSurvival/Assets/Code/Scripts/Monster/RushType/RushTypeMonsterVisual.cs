using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RushTypeMonsterVisual : NormalMonsterVisual
{
    private int _rushSpeedHash; // 애니메이션 파라미터의 이름
    private float _maxSpeed;
    private float initialSpeed; // 초기 속도
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
