using UnityEngine;

public class PlayerAnimator : CharacterAnimator
{
    public PlayerAnimator(Animator animator) : base(animator)
    {
    }

    public void SetAiming(bool value)
    {
        AnimatorComponent.SetBool(PlayerAnimatorData.Params.IsAiming, value);
    }

    public void SetGrounded(bool value)
    {
        AnimatorComponent.SetBool(PlayerAnimatorData.Params.IsGrounded, value);
    }

    public class PlayerAnimatorData
    {
        public class Params
        {
            public static readonly int IsAiming = Animator.StringToHash(nameof(IsAiming));
            public static readonly int IsGrounded = Animator.StringToHash(nameof(IsGrounded));
        }
    }
}
