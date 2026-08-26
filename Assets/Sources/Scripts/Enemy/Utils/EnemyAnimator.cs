using UnityEngine;

public class EnemyAnimator : CharacterAnimator
{
    public EnemyAnimator(Animator animator) : base(animator) { }

    public void SetMeleeAttack()
    {
        AnimatorComponent.SetTrigger(EnemyAnimatorData.Params.MeleeAttack);
    }

    public void SetRangedAttack()
    {
        AnimatorComponent.SetTrigger(EnemyAnimatorData.Params.RangedAttack);
    }

    public void SetWalking(bool value)
    {
        AnimatorComponent.SetBool(EnemyAnimatorData.Params.IsWalking, value);
    }

    public void SetAiming(bool value)
    {
        AnimatorComponent.SetBool(EnemyAnimatorData.Params.IsAiming, value);
    }

    public void SetCast()
    {
        AnimatorComponent.SetTrigger(EnemyAnimatorData.Params.Cast);
    }

    public class EnemyAnimatorData
    {
        public class Params
        {
            public static readonly int MeleeAttack = Animator.StringToHash(nameof(MeleeAttack));
            public static readonly int RangedAttack = Animator.StringToHash(nameof(RangedAttack));
            public static readonly int Cast = Animator.StringToHash(nameof(Cast));
            public static readonly int IsWalking = Animator.StringToHash(nameof(IsWalking));
            public static readonly int IsAiming = Animator.StringToHash(nameof(IsAiming));
        }
    }
}
