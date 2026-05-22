using UnityEngine;

public abstract class CharacterAnimator
{
    private Animator _animator;

    public Animator AnimatorComponent => _animator;

    public CharacterAnimator(Animator animator)
    {
        _animator = animator;
    }

    public void SetDied(bool value)
    {
        AnimatorComponent.SetBool(CharacterAnimatorData.Params.IsDied, value);
    }

    public class CharacterAnimatorData
    {
        public class Params
        {
            public static readonly int IsDied = Animator.StringToHash(nameof(IsDied));
        }
    }
}
