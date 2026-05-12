using UnityEngine;
using Zenject;

[RequireComponent(typeof(BloodEffectSpawner))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Enemy : MonoBehaviour, IResetable
{
    [SerializeField] EnemyAttacker _attacker;

    [Inject] private LevelState _levelState;
    
    private EnemyAnimator _animator;
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private BloodEffectSpawner _effect;
    private Transform _transform;
    private Transform _spawnPoint;
    
    private void Awake()
    {
        _transform = transform;
        _animator = new EnemyAnimator(GetComponent<Animator>());
        _effect = GetComponent<BloodEffectSpawner>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
    }

    [Inject]
    private void Construct(Transform spawnPoint)
    {
        _spawnPoint = spawnPoint;
    }

    public void Attack()
    {
        _animator.SetAttack();    
    }

    public void Die(ContactPoint hitPoint)
    {
        _effect.Perform(hitPoint);

        _attacker.Disable();
        
        _animator.SetDied(true);        
        _collider.enabled = false;

        _levelState.CurrentEnemiesCount.Value--;
    }

    public void ResetOnRestart()
    {
        _transform.position = _spawnPoint.position;
        _transform.rotation = _spawnPoint.rotation;

        _attacker.Enable();

        _animator.SetDied(false);        
        _collider.enabled = true;
    }
}
