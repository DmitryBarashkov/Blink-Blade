using UnityEngine;

public class PlayerAnimator
{
    private Animator _animator;
    
    public PlayerAnimator(Animator animator)
    {
        _animator = animator;
    }

    public void SetAiming(bool value)
    {
        _animator.SetBool(PlayerAnimatorData.Params.IsAiming, value);
    }

    public class PlayerAnimatorData
    {
        public class Params
        {
            public static readonly int IsAiming = Animator.StringToHash(nameof(IsAiming));
        }
    }
}
