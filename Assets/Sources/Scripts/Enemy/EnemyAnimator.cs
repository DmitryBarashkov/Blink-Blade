using System;
using UnityEngine;

public class EnemyAnimator : CharacterAnimator
{
    public EnemyAnimator(Animator animator) : base(animator)
    {
    }

    public void SetDied(bool value)
    {
        AnimatorComponent.SetBool(EnemyAnimatorData.Params.IsDied, value);
    }

    public void SetAttack()
    {
        AnimatorComponent.SetTrigger(EnemyAnimatorData.Params.Attack);
    }

    public class EnemyAnimatorData
    {
        public class Params
        {
            public static readonly int IsDied = Animator.StringToHash(nameof(IsDied));           
            public static readonly int Attack = Animator.StringToHash(nameof(Attack));           
        }
    }
}
