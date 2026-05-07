using UnityEngine;

public abstract class CharacterAnimator
{
    private Animator _animator;

    public Animator AnimatorComponent => _animator;

    public CharacterAnimator(Animator animator)
    {
        _animator = animator;
    }

    public float GetAnimationLength()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).length;
    }
}
