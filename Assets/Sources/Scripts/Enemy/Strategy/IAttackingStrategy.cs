using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackingStrategy
{
    event Action AttackStarted;
    event Action AttackStopped;
    void Initialize(EnemyAttacker attacker, IAudioService audioService, EnemyAnimator animator);
    void Enable();    
    void Disable();
}

public class MeleeAttack : IAttackingStrategy
{
    public event Action AttackStarted;
    public event Action AttackStopped;
    
    private EnemyAttacker _attacker;
    private IAudioService _audioService;
    private EnemyAnimator _animator;
    
    public void Initialize(EnemyAttacker attacker, IAudioService audioService, EnemyAnimator animator)
    {
        _attacker = attacker;
        _audioService = audioService;
        _animator = animator;
    }

    public void Enable()
    {
        _attacker.Enable();
        _attacker.OnPlayerInAttackArea += Attack;
        _attacker.OnPlayerOutAttackArea += StopAttack;
    }

    public void Disable()
    {
        _attacker.Disable();
        _attacker.OnPlayerInAttackArea -= Attack;
        _attacker.OnPlayerOutAttackArea -= StopAttack;
    }

    private void Attack()
    {
        _animator.SetAttack();
        _audioService.PlaySound(SoundType.SwordAttack);
        AttackStarted?.Invoke();
    }

    private void StopAttack()
    {
        AttackStopped?.Invoke();
    }

}
