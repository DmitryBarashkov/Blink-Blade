using UnityEngine;
using Zenject;

[RequireComponent(typeof(HitEffectSpawner))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour, IResetable
{
    [SerializeField] EnemyAttacker _attacker;
    
    protected EnemyAnimator _animator;    
    protected Transform _transform;
    private Vector3 _initiatePosition;
    private Quaternion _initiateRotation;

    private LevelState _levelState;
    private CapsuleCollider _collider;    
    private IAudioService _audioService;    
    
    [Inject]
    public virtual void Construct(IAudioService audioService, LevelState levelState)
    {
        _transform = transform;
        _audioService = audioService;
        _levelState = levelState;
    }

    protected virtual void Awake()
    {
        _animator = new EnemyAnimator(GetComponent<Animator>());
        _collider = GetComponent<CapsuleCollider>();
    }

    public virtual void Activate()
    {
        _transform.position = _initiatePosition;
        _transform.rotation = _initiateRotation;

        _attacker.Enable();

        _animator.SetDied(false);
        _collider.enabled = true;
    }
    
    public virtual void Attack()
    {
        _animator.SetAttack();
        _audioService.PlaySound(SoundType.SwordAttack);
    }

    public virtual void Die(ContactPoint hitPoint)
    {
        _attacker.Disable();        
        
        _animator.SetDied(true);        
        _collider.enabled = false;

        _levelState.CurrentEnemiesCount.Value--;
    }

    public void ResetOnRestart()
    {
        Activate();
    }

    public void SetInitiatePosition(Transform initTransform)
    {
        _transform.position = _initiatePosition = initTransform.position;
        _transform.rotation = _initiateRotation = initTransform.rotation;
    }

    public class Factory : PlaceholderFactory<Object, Enemy> { }
}
