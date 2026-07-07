using System;

public interface IAttackingStrategy
{
    event Action AttackStarted;
    event Action AttackStopped;
    void Initialize(MeleeAttacker meleeAttacker, RangedAttacker rangedAttacker, IAudioService audioService, 
                    EnemyAnimator animator,
                    Enemy enemy, Player player, ObjectPoolService poolService);
    void Activate();
    void Deactivate();
    void Tick();
}
