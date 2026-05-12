using System;
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

    public class EnemyAnimatorData
    {
        public class Params
        {
            public static readonly int Attack = Animator.StringToHash(nameof(Attack));           
        }
    }
}
