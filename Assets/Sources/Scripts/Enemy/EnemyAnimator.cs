using UnityEngine;

public class EnemyAnimator : CharacterAnimator
{
    public EnemyAnimator(Animator animator) : base(animator)
    {
    }

    public void SetAttack()
    {
        AnimatorComponent.SetTrigger(EnemyAnimatorData.Params.Attack);
    }

    public void SetWalking(bool value)
    {
        AnimatorComponent.SetBool(EnemyAnimatorData.Params.IsWalking, value);
    }

    public class EnemyAnimatorData
    {
        public class Params
        {
            public static readonly int Attack = Animator.StringToHash(nameof(Attack));          
            public static readonly int IsWalking = Animator.StringToHash(nameof(IsWalking));          
        }
    }
}
