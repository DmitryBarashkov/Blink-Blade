using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyAnimator _animator;
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private EffectSpawner _effect;
    
    private void Awake()
    {
        _animator = new EnemyAnimator(GetComponent<Animator>());
        _effect = GetComponent<EffectSpawner>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
    }

    public void Die(ContactPoint hitPoint)
    {
        _effect.Perform(hitPoint);
        
        _animator.SetDied(true);
        _rigidbody.isKinematic = true;
        _collider.enabled = false;        
    }
}
