using UnityEngine;
using UnityEngine.Animations.Rigging;
using Zenject;

[RequireComponent(typeof(HitEffectSpawner))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private MeleeAttacker _meleeAttacker;
    [SerializeField] private RangedAttacker _rangedAttacker;

    [Header("Moving")]
    [SerializeField] private float _wallCheckDistance = 1f;
    [SerializeField] private float _cliffForwardOffset = 0.4f;

    [Header("Defence")]
    [SerializeField] private Blocker _blocker;
    [SerializeField] private Shield _shield;

    private bool _isDead = false;

    private int _initialHealth = 1;
    private int _health = 1;

    private Transform _transform;
    private CapsuleCollider _collider;
    private Animator _animator;
    private EnemyAnimator _enemyAnimator;
    private RigBuilder _rigBuilder;
    private LevelState _levelState;
    private ILevelData _levelData;

    private IMovementStrategy _movementStrategy;
    private IAttackingStrategy _attackingStrategy;
    private IDefendingStrategy _defendingStrategy;

    private Vector3 _initiatePosition;
    private Quaternion _initiateRotation;

    private IAudioService _audioService;
    private ObjectPoolService _poolService;

    public EnemyAnimator AnimatorInstance => _enemyAnimator;
    public bool IsDead => _isDead;

    [Inject]
    public void Construct(IAudioService audioService,
                          IMovementStrategy movementStrategy,
                          IAttackingStrategy attackingStrategy,
                          IDefendingStrategy defendingStrategy,
                          ILevelData levelData,
                          LevelState levelState,
                          ObjectPoolService poolService)
    {
        _transform = transform;

        _audioService = audioService;
        _poolService = poolService;

        _movementStrategy = movementStrategy;
        _attackingStrategy = attackingStrategy;
        _defendingStrategy = defendingStrategy;

        _levelState = levelState;
        _levelData = levelData;

        _initiatePosition = _transform.position;
        _initiateRotation = _transform.rotation;

        if (_levelData.IsBossLevel())
            _initialHealth = levelData.GetBossHealth();
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemyAnimator = new EnemyAnimator(GetComponent<Animator>());
        _collider = GetComponent<CapsuleCollider>();
        _rigBuilder = GetComponent<RigBuilder>();



        _movementStrategy.Initialize(_transform, _collider, _enemyAnimator, _levelData, _audioService, _wallCheckDistance, _cliffForwardOffset);
        _attackingStrategy.Initialize(_meleeAttacker, _rangedAttacker, _audioService, _enemyAnimator, this, _poolService);
        _defendingStrategy.Initialize(_animator, _rigBuilder, _blocker, _shield);
    }

    private void OnEnable()
    {
        _movementStrategy.MovementStarted += OnStartMoving;

        _attackingStrategy.AttackStarted += StopMove;
        _attackingStrategy.AttackStopped += KeepMove;

        _defendingStrategy.StartBlocking += KeepMove;
    }

    private void OnDisable()
    {
        _attackingStrategy.AttackStarted -= StopMove;
        _attackingStrategy.AttackStopped -= KeepMove;
        _attackingStrategy.Deactivate();

        _defendingStrategy.StartBlocking -= KeepMove;
        _defendingStrategy.Deactivate();
    }

    private void Update()
    {
        _movementStrategy.Tick();
        _attackingStrategy.Tick();
    }

    public void Activate()
    {
        _transform.position = _initiatePosition;
        _transform.rotation = _initiateRotation;
        _health = _initialHealth;
        _isDead = false;

        StartWork();

        AnimatorInstance.SetDied(false);
    }

    public void StartWork()
    {
        _collider.enabled = true;

        _attackingStrategy.Activate();
        _defendingStrategy.Activate();
        _movementStrategy.Activate();
    }

    public void Deactivate()
    {
        _attackingStrategy.Deactivate();
        _defendingStrategy.Deactivate();
        _collider.enabled = false;
    }

    public void TakeDamage()
    {
        _health--;

        if (_health <= 0)
        {
            Die();
        }
        else
        {
            _collider.enabled = false;
            _movementStrategy.Perform();
            _collider.enabled = true;

            _levelState.CurrentEnemiesCount.Value--;
        }
    }

    private void StopMove()
    {
        _movementStrategy.Stop();
    }

    private void KeepMove()
    {
        _movementStrategy.KeepMoving();
    }

    private void OnStartMoving()
    {
        _defendingStrategy.StopBlock();
    }

    private void Die()
    {
        _movementStrategy.Deactivate();
        _attackingStrategy.Deactivate();
        _defendingStrategy.Deactivate();

        _collider.enabled = false;
        _isDead = true;

        AnimatorInstance.SetDied(true);

        _levelState.CurrentEnemiesCount.Value--;
    }
}
