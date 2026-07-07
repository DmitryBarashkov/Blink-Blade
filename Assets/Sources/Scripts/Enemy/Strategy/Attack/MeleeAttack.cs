using System;
using UnityEngine;

public class MeleeAttack : IAttackingStrategy
{
    public event Action AttackStarted;
    public event Action AttackStopped;

    private MeleeAttacker _attacker;
    private IAudioService _audioService;
    private EnemyAnimator _animator;

    private bool _isActive;
    private float _cooldownTimer = 0f;
    private float _cooldown = 1f;

    public void Initialize(MeleeAttacker meleeAttacker, RangedAttacker rangedAttacker, IAudioService audioService,
                           EnemyAnimator animator,
                           Enemy enemy, Player player, ObjectPoolService poolService)
    {
        _attacker = meleeAttacker;
        _audioService = audioService;
        _animator = animator;
    }

    public void Activate()
    {
        _attacker.OnPlayerInAttackArea += Attack;
        _attacker.Activate();
    }

    public void Deactivate()
    {
        _attacker.OnPlayerInAttackArea -= Attack;        
        _attacker.Deactivate();
    }

    public void Tick() 
    { 
        if (_isActive)
        {
            _cooldownTimer += Time.deltaTime;

            if (_cooldownTimer >=  _cooldown)
            {
                StopAttack();
            }
        }
    }

    private void Attack()
    {
        _animator.SetMeleeAttack();
        _audioService.PlaySound(SoundType.SwordAttack);
        _isActive = true;
        
        AttackStarted?.Invoke();
    }

    private void StopAttack()
    {
        AttackStopped?.Invoke();
        _isActive = false;
        _cooldownTimer = 0;
    }
}
