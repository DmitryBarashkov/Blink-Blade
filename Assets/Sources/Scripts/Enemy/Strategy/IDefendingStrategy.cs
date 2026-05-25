using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public interface IDefendingStrategy
{
    event Action StartBlocking;    
    
    void Initialize(Animator animator, RigBuilder rigBuilder, Blocker blocker, Shield shield);
    void Activate();
    void Deactivate();
    void StopBlock();
}

[Serializable]
public class Defenceless : IDefendingStrategy
{
    public event Action StartBlocking = delegate { };    

    public void Initialize(Animator animator, RigBuilder rigBuilder, Blocker blocker, Shield shield) { }
    public void Activate() { }
    public void Deactivate() { }
    
    public void StopBlock() { }
}

[Serializable]
public class ShieldDefense : IDefendingStrategy
{
    public event Action StartBlocking;    

    private Blocker _blocker;
    private Shield _shield;

    private RigBuilder _rigBuilder;
    private ShieldEnemyAnimator _animator;

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
        _blocker.Reset();

        _shield.OnHitWeapon += ShieldImpact;
        _shield.Activate();

        _rigBuilder.enabled = true;
    }

    public void Deactivate()
    {
        _blocker.OnWeaponInBlockingArea -= StartBlock;
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
