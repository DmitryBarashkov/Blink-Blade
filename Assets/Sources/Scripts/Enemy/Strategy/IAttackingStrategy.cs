using System;
using UnityEngine;

public interface IAttackingStrategy
{
    event Action AttackStarted;
    event Action AttackStopped;
    void Initialize(EnemyAttacker attacker, IAudioService audioService, 
                    EnemyAnimator animator,
                    Transform transform, Transform target, ObjectPoolService poolService);
    void Activate();
    void Deactivate();
    void Tick();
}

public class MeleeAttack : IAttackingStrategy
{
    public event Action AttackStarted;
    public event Action AttackStopped;
    
    private EnemyAttacker _attacker;
    private IAudioService _audioService;
    private EnemyAnimator _animator;
    
    public void Initialize(EnemyAttacker attacker, IAudioService audioService, 
                           EnemyAnimator animator,
                           Transform transform, Transform target, ObjectPoolService poolService)
    {
        _attacker = attacker;
        _audioService = audioService;
        _animator = animator;
    }

    public void Activate()
    {
        _attacker.Activate();
        _attacker.OnPlayerInAttackArea += Attack;
        _attacker.OnPlayerOutAttackArea += StopAttack;
    }

    public void Deactivate()
    {
        _attacker.Deactivate();
        _attacker.OnPlayerInAttackArea -= Attack;
        _attacker.OnPlayerOutAttackArea -= StopAttack;
    }

    public void Tick() { }

    private void Attack()
    {
        _animator.SetAttack();
        _audioService.PlaySound(SoundType.SwordAttack);
        AttackStarted?.Invoke();
    }

    private void StopAttack()
    {
        AttackStopped?.Invoke();
    }
}

public class RangedAttack : IAttackingStrategy
{
    public event Action AttackStarted;
    public event Action AttackStopped;

    private ObjectPoolService _poolService;
    
    private EnemyAttacker _attacker;
    private IAudioService _audioService;
    private EnemyAnimator _animator;
    private Transform _enemyTransform;
    private Transform _playerTransform;

    private AttackState _attackState; 
    private bool _isActive = false;
    private float _aimingCooldown = 2f;
    private float _cooldownTimer = 0f;
    private float _viewAngle = 120f;
    private float _shootForce = 15f;
    private LayerMask _obstacleLayer;

    public void Initialize(EnemyAttacker attacker, IAudioService audioService, 
                           EnemyAnimator animator, 
                           Transform transform, Transform target, ObjectPoolService poolService)
    {
        _enemyTransform = transform;
        _playerTransform = target;
        _poolService = poolService;

        _attacker = attacker;
        _audioService = audioService;
        _animator = animator;
        _obstacleLayer = LayerMask.GetMask("Ground");        
    }

    public void Activate()
    {
        _attacker.Activate();
        _attackState = AttackState.Idle;

        _isActive = true;
        _animator.SetAiming(false);
        _cooldownTimer = 0;

        _attacker.OnPlayerInAttackArea += TryAim;
        _attacker.OnPlayerOutAttackArea += StopTryAim;
    }

    public void Deactivate()
    {
        _attacker.Deactivate();
        _attackState = AttackState.Idle;

        _isActive = false;
        
        _attacker.OnPlayerInAttackArea -= TryAim;
        _attacker.OnPlayerOutAttackArea -= StopTryAim;
    }

    public void Tick()
    {
        if (_isActive == false)
            return;
        
        if (_playerTransform == null || _enemyTransform == null)
            throw new ArgumentNullException("Attacking strategy transform");

        if (_attackState != AttackState.Idle)
        {
            bool isSeePlayer = CanSeePlayer(_enemyTransform, _playerTransform);

            if (isSeePlayer)
                Aim();
            else
                TryAim();
        }
    }

    private void TryAim()
    {
        _cooldownTimer = 0;
        _attackState = AttackState.TryAiming;
        _animator.SetAiming(false);
    }

    private void Aim()
    {
        if (_cooldownTimer == 0)
        {
            AttackStarted?.Invoke();
            _animator.SetAiming(true);
            _attackState = AttackState.Aiming;
            _audioService.PlaySound(SoundType.ArcherStartAim);
        }

        _cooldownTimer += Time.deltaTime;

        if (_cooldownTimer >= _aimingCooldown)
        {
            Shoot();
            _cooldownTimer = 0;
        }
    }

    private void StopTryAim()
    {
        _attackState = AttackState.Idle;
        _cooldownTimer = 0f;

        _animator.SetAiming(false);
        AttackStopped?.Invoke();
    }

    private void Shoot()
    {
        Vector3 startPosition = _enemyTransform.position + Vector3.up;
        Vector3 targetPosition = _playerTransform.position + Vector3.up;
        Vector3 direction = (targetPosition - startPosition).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject arrow = _poolService.Get(ObjectPoolService.PoolObjectTypes.Arrow, startPosition, rotation);
        Rigidbody rigidbody = arrow.GetComponent<Rigidbody>();

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.AddForce(direction * _shootForce, ForceMode.Impulse);

        _attackState = AttackState.Shoot;
        _animator.SetAttack();
        _audioService.PlaySound(SoundType.BowShot);
    }

    private bool CanSeePlayer(Transform enemyTransform, Transform target)
    {
        if (target == null) 
            return false;

        Vector3 startPos = enemyTransform.position + Vector3.up;
        Vector3 targetPos = target.position + Vector3.up;
        Vector3 directionToPlayer = targetPos - startPos;
        Vector3 lookDirection = enemyTransform.forward;
        float angleToPlayer = Vector3.Angle(lookDirection, directionToPlayer);

        Debug.DrawRay(startPos, directionToPlayer, Color.red);

        if (angleToPlayer > _viewAngle / 2f)
        {
            return false;
        }

        float distance = Vector3.Distance(startPos, targetPos);
        RaycastHit hit;

        Debug.DrawRay(startPos, directionToPlayer, Color.red);
        return Physics.Raycast(startPos, directionToPlayer.normalized, out hit, distance, _obstacleLayer) == false;
    }

    private enum AttackState
    {
        Idle,
        TryAiming,
        Aiming,
        Shoot
    }
}
