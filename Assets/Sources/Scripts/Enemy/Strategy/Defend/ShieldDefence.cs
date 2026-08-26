using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[Serializable]
public class ShieldDefence : IDefendingStrategy
{
    private Blocker _blocker;
    private Shield _shield;

    private RigBuilder _rigBuilder;
    private ShieldEnemyAnimator _animator;

    public event Action StartBlocking;

    public void Initialize(Animator animator, RigBuilder rigBuilder, Blocker blocker, Shield shield)
    {
        _animator = new ShieldEnemyAnimator(animator);
        _rigBuilder = rigBuilder;
        _blocker = blocker;
        _shield = shield;
    }

    public void Activate()
    {
        _blocker.OnWeaponInBlockingArea += StartBlock;
        _blocker.OnWeaponOutBlockingArea += StopBlock;
        _blocker.Reset();

        StopBlock();

        _shield.OnHitWeapon += ShieldImpact;
        _shield.Activate();

        _rigBuilder.enabled = true;
    }

    public void Deactivate()
    {
        _blocker.OnWeaponInBlockingArea -= StartBlock;
        _blocker.OnWeaponOutBlockingArea -= StopBlock;
        _blocker.Reset();

        _shield.OnHitWeapon -= ShieldImpact;
        _shield.Deactivate();

        _rigBuilder.enabled = false;
    }

    public void StopBlock()
    {
        _animator.SetBlocking(false);
    }

    private void StartBlock()
    {
        _animator.SetBlocking(true);
        StartBlocking?.Invoke();
    }

    private void ShieldImpact()
    {
        _animator.BlockImpact();
        StartBlocking?.Invoke();
    }
}
