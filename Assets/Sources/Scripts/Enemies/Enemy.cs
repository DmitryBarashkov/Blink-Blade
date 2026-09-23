using System;
using BlinkBlade.Game;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Zenject;

namespace BlinkBlade.Enemies
{
    [RequireComponent(typeof(HitEffectSpawner))]
    [RequireComponent(typeof(Animator))]
    public class Enemy : MonoBehaviour
    {
        [Header("Attack")]
        [SerializeField] private EnemyAttacker _attacker;

        [Header("Defence")]
        [SerializeField] private Blocker _blocker;

        [Header("Health")]
        [SerializeField] private int _health = 1;

        private ILevelData _levelData;

        private bool _isDead = false;
        private int _initialHealth;

        private Transform _transform;
        private CapsuleCollider _collider;
        private Animator _animator;
        private EnemyAnimator _enemyAnimator;
        private RigBuilder _rigBuilder;
        private Mover _mover;
        private LevelState _levelState;

        private Vector3 _initiatePosition;
        private Quaternion _initiateRotation;

        private IAudioService _audioService;
        private ObjectPoolService _poolService;

        public EnemyAnimator AnimatorInstance => _enemyAnimator;

        public bool IsDead => _isDead;

        [Inject]
        public void Construct(
            IAudioService audioService,
            ILevelData levelData,
            LevelState levelState,
            ObjectPoolService poolService)
        {
            _transform = transform;

            _audioService = audioService;
            _poolService = poolService;

            _levelState = levelState;
            _levelData = levelData;

            _initiatePosition = _transform.position;
            _initiateRotation = _transform.rotation;

            if (_levelData != null && _levelData.IsBossLevel())
                _initialHealth = _levelData.GetBossHealth();
            else
                _initialHealth = _health;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _enemyAnimator = new EnemyAnimator(GetComponent<Animator>());
            _collider = GetComponent<CapsuleCollider>();
            _rigBuilder = GetComponent<RigBuilder>();
            _mover = GetComponent<Mover>();

            if (_attacker == null)
                throw new ArgumentNullException(nameof(_attacker));

            if (_mover != null)
                _mover.Initialize(_transform, _collider, _enemyAnimator, _levelData, _audioService);

            if (_blocker != null)
                _blocker.Initialize(_animator, _rigBuilder);

            _attacker.Initialize(_audioService, _enemyAnimator, _poolService, this);
        }

        private void OnEnable()
        {
            if (_mover != null)
                _mover.MovementStarted += OnStartMoving;

            if (_blocker != null)
                _blocker.StartBlocking += OnAttackStoped;

            _attacker.AttackStarted += OnAttackStarted;
            _attacker.AttackStopped += OnAttackStoped;
        }

        private void OnDisable()
        {
            _attacker.AttackStarted -= OnAttackStarted;
            _attacker.AttackStopped -= OnAttackStoped;
            _attacker.Deactivate();

            if (_mover != null)
                _mover.MovementStarted -= OnStartMoving;

            if (_blocker != null)
            {
                _blocker.StartBlocking -= OnAttackStoped;
                _blocker.Deactivate();
            }
        }

        public void Activate()
        {
            _transform.position = _initiatePosition;
            _transform.rotation = _initiateRotation;
            _health = _initialHealth;
            _isDead = false;

            _collider.enabled = true;

            _attacker.Activate();

            if (_mover != null)
                _mover.Activate();

            if (_blocker != null)
                _blocker.Activate();

            AnimatorInstance.SetDied(false);
        }

        public void ContinueWork()
        {
            _collider.enabled = true;

            _attacker.Activate();

            if (_mover != null)
                _mover.KeepMoving();

            if (_blocker != null)
                _blocker.Activate();
        }

        public void Deactivate()
        {
            _attacker.Deactivate();

            if (_mover != null)
                _mover.Deactivate();

            if (_blocker != null)
                _blocker.Deactivate();

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
                _mover.Move();
                _collider.enabled = true;

                _levelState.CurrentEnemiesCount.Value--;
            }
        }

        private void OnAttackStarted()
        {
            if (_mover != null)
                _mover.Stop();
        }

        private void OnAttackStoped()
        {
            if (_mover != null)
                _mover.KeepMoving();
        }

        private void OnStartMoving()
        {
            if (_blocker != null)
                _blocker.StopBlock();
        }

        private void Die()
        {
            Deactivate();

            _isDead = true;

            AnimatorInstance.SetDied(true);

            _levelState.CurrentEnemiesCount.Value--;
        }
    }
}