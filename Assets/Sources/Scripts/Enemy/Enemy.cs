using UnityEngine;
using Zenject;

[RequireComponent(typeof(HitEffectSpawner))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour, IResetable
{
    [SerializeField] EnemyAttacker _attacker;
    [SerializeField] float _wallCheckDistance;
    
    private Transform _transform;
    private CapsuleCollider _collider;
    private EnemyAnimator _animator;
    private LevelState _levelState;

    private IMovementStrategy _movementStrategy;
    private IAttackingStrategy _attackingStrategy;

    private Vector3 _initiatePosition;
    private Quaternion _initiateRotation;

    private IAudioService _audioService;

    public EnemyAnimator AnimatorInstance => _animator;

    [Inject]
    public void Construct(IAudioService audioService, 
                          IMovementStrategy movementStrategy, 
                          IAttackingStrategy attackingStrategy, 
                          LevelState levelState)
    {
        _transform = transform;        
        _audioService = audioService;        
        
        _movementStrategy = movementStrategy;
        _attackingStrategy = attackingStrategy;

        _levelState = levelState;

        _initiatePosition = _transform.position;
        _initiateRotation = _transform.rotation;
    }

    private void Awake()
    {
        _animator = new EnemyAnimator(GetComponent<Animator>());
        _collider = GetComponent<CapsuleCollider>();

        _movementStrategy.Initialize(_transform, _animator, _wallCheckDistance);
        _attackingStrategy.Initialize(_attacker, _audioService, _animator);
    }

    private void OnEnable()
    {
        _attackingStrategy.AttackStarted += Attack;
        _attackingStrategy.AttackStopped += StopAttack;
    }

    private void OnDisable()
    {
        _attackingStrategy.AttackStarted -= Attack;
        _attackingStrategy.AttackStopped -= StopAttack;
        _attackingStrategy.Disable();
    }

    private void Update()
    {
        _movementStrategy.Tick();
    }

    public void Activate()
    {
        _transform.position = _initiatePosition;
        _transform.rotation = _initiateRotation;

        _attackingStrategy.Enable();
        _movementStrategy.Start();

        AnimatorInstance.SetDied(false);
        _collider.enabled = true;
    }
    
    public void Attack()
    {
        _movementStrategy.Stop();
    }

    public void StopAttack()
    {
        _movementStrategy.KeepMoving();
    }

    public void Die(ContactPoint hitPoint)
    {
        _movementStrategy.Stop();
        _attackingStrategy.Disable();

        _collider.enabled = false;

        AnimatorInstance.SetDied(true);

        _levelState.CurrentEnemiesCount.Value--;
    }

    public void ResetOnRestart()
    {
        Activate();
    }
}
