using UnityEngine;

public abstract class CharacterAnimator
{
    private Animator _animator;

    public Animator AnimatorComponent => _animator;

    public CharacterAnimator(Animator animator)
    {
        _animator = animator;
    }    
}
