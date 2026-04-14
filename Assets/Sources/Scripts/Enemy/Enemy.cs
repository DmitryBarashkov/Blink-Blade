using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyAnimator _animator;
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    
    private void Awake()
    {
        _animator = new EnemyAnimator(GetComponent<Animator>());
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
    }

    public void Die()
    {
        _animator.SetDied(true);
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
        this.enabled = false;
    }
}
