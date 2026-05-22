using UnityEngine;

public class ShieldEnemyAnimator : EnemyAnimator
{
    public ShieldEnemyAnimator(Animator animator) : base(animator)
    {
    }

    public void SetBlocking(bool value)
    {
        AnimatorComponent.SetBool(ShieldEnemyAnimatorData.Params.IsBlocking, value);
    }

    public void BlockImpact()
    {
        AnimatorComponent.SetTrigger(ShieldEnemyAnimatorData.Params.BlockImpact);
    }

    public class ShieldEnemyAnimatorData
    {
        public class Params
        {
            public static readonly int IsBlocking = Animator.StringToHash(nameof(IsBlocking));
            public static readonly int BlockImpact = Animator.StringToHash(nameof(BlockImpact));
        }
    }
}
