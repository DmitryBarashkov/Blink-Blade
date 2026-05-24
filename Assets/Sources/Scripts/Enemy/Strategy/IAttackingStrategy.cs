using System;

public interface IAttackingStrategy
{
    event Action AttackStarted;
    event Action AttackStopped;
    void Initialize(EnemyAttacker attacker, IAudioService audioService, EnemyAnimator animator);
    void Activate();    
    void Deactivate();
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

    public void Activate()
    {
        _attacker.Activate();
        _attacker.OnPlayerInAttackArea += Attack;
        _attacker.OnPlayerOutAttackArea += StopAttack;
    }

    public void Deactivate()
    {
        _attacker.Deactivate();
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
