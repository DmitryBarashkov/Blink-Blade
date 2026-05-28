using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Zenject;

[RequireComponent(typeof(HitEffectSpawner))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour, IResetable
{
    [Header("Attack")]
    [SerializeField] EnemyAttacker _attacker;

    [Header("Moving")]
    [SerializeField] float _wallCheckDistance = 1f;
    [SerializeField] float _cliffForwardOffset = 0.4f;
    
    [Header(("Defence"))]
    [SerializeField] private Blocker _blocker;
    [SerializeField] private Shield _shield;

    private Transform _transform;
    private Player _player;
    private CapsuleCollider _collider;
    private Animator _animator;
    private EnemyAnimator _enemyAnimator;
    private RigBuilder _rigBuilder;
    private LevelState _levelState;

    private IMovementStrategy _movementStrategy;
    private IAttackingStrategy _attackingStrategy;
    private IDefendingStrategy _defendingStrategy;

    private Vector3 _initiatePosition;
    private Quaternion _initiateRotation;

    private IAudioService _audioService;
    private ObjectPoolService _poolService;

    public EnemyAnimator AnimatorInstance => _enemyAnimator;

    [Inject]
    public void Construct(IAudioService audioService, 
                          IMovementStrategy movementStrategy, 
                          IAttackingStrategy attackingStrategy,
                          IDefendingStrategy defendingStrategy,
                          LevelState levelState,
                          Player player,
                          ObjectPoolService poolService)
    {
        _transform = transform;
        _player = player;
        _audioService = audioService;
        _poolService = poolService;
        
        _movementStrategy = movementStrategy;
        _attackingStrategy = attackingStrategy;
        _defendingStrategy = defendingStrategy;

        _levelState = levelState;

        _initiatePosition = _transform.position;
        _initiateRotation = _transform.rotation;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemyAnimator = new EnemyAnimator(GetComponent<Animator>());
        _collider = GetComponent<CapsuleCollider>();
        _rigBuilder = GetComponent<RigBuilder>();

        _movementStrategy.Initialize(_transform, _enemyAnimator, _wallCheckDistance, _cliffForwardOffset);
        _attackingStrategy.Initialize(_attacker, _audioService, _enemyAnimator, this, _player, _poolService);
        _defendingStrategy.Initialize(_animator, _rigBuilder, _blocker, _shield);
    }

    private void OnEnable()
    {
        _movementStrategy.MovementStarted += OnStartMoving;
        
        _attackingStrategy.AttackStarted += StopMove;
        _attackingStrategy.AttackStopped += KeepMove;

        _defendingStrategy.StartBlocking += KeepMove;

        _player.Dead += Deactivate;
    }

    private void OnDisable()
    {
        _attackingStrategy.AttackStarted -= StopMove;
        _attackingStrategy.AttackStopped -= KeepMove;
        _attackingStrategy.Deactivate();

        _defendingStrategy.StartBlocking -= KeepMove;        
        _defendingStrategy.Deactivate();

        _player.Dead -= Deactivate;
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

        _attackingStrategy.Activate();
        _defendingStrategy.Activate();
        _movementStrategy.Activate();

        AnimatorInstance.SetDied(false);
        _collider.enabled = true;
    }
    
    public void Die(ContactPoint hitPoint)
    {
        _movementStrategy.Deactivate();
        _attackingStrategy.Deactivate();
        _defendingStrategy.Deactivate();

        _collider.enabled = false;

        AnimatorInstance.SetDied(true);

        _levelState.CurrentEnemiesCount.Value--;
    }

    public void ResetOnRestart()
    {
        Activate();
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

    private void Deactivate()
    {
        _attackingStrategy.Deactivate();
        _defendingStrategy.Deactivate();        
        _collider.enabled = false;
    }
}
