using System;
using UnityEngine;

public class RangedAttack : IAttackingStrategy
{
    private ObjectPoolService _poolService;

    private RangedAttacker _attacker;
    private IAudioService _audioService;
    private EnemyAnimator _animator;
    private Transform _enemyTransform;
    private Transform _playerTransform;

    private AttackState _attackState;
    private bool _isActive = false;
    private float _aimingCooldown = 1.2f;
    private float _cooldownTimer = 0f;
    private float _viewAngle = 120f;
    private float _shootForce = 15f;
    private float _aimHeightFactor = 0.7f;
    private float _checkHitRadius = 0.2f;
    private Vector3 _playerCenterPosition;
    private Vector3 _enemyCenterPosition;
    private LayerMask _obstacleLayer;
    private Vector3 _lockTargetPosition;

    public event Action AttackStarted;

    public event Action AttackStopped;

    private enum AttackState
    {
        Idle,
        TryAiming,
        Aiming,
        Shoot,
    }

    public void Initialize(
        MeleeAttacker meleeAttacker,
        RangedAttacker rangedAttacker,
        IAudioService audioService,
        EnemyAnimator animator,
        Enemy enemy,
        ObjectPoolService poolService)
    {
        _enemyTransform = enemy.transform;
        _enemyCenterPosition = Vector3.up * enemy.GetComponent<CapsuleCollider>().height * _enemyTransform.lossyScale.y * _aimHeightFactor;
        _poolService = poolService;

        _attacker = rangedAttacker;
        _audioService = audioService;
        _animator = animator;
        _obstacleLayer = LayerMask.GetMask("Ground");
    }

    public void Activate()
    {
        _attacker.OnPlayerInAttackArea += SetAim;
        _attacker.OnPlayerOutAttackArea += StopTryAim;

        _attacker.Activate();

        _isActive = true;
        _animator.SetAiming(false);
        _cooldownTimer = 0;
    }

    public void Deactivate()
    {
        _attacker.OnPlayerInAttackArea -= SetAim;
        _attacker.OnPlayerOutAttackArea -= StopTryAim;

        _attacker.Deactivate();
        _attackState = AttackState.Idle;

        _isActive = false;
    }

    public void Tick()
    {
        if (_isActive == false || _playerTransform == null || _enemyTransform == null)
            return;

        if (_attackState != AttackState.Idle)
        {
            bool isSeePlayer = CanSeePlayer();

            if (isSeePlayer)
                Aim();
            else
                TryAim();
        }
    }

    private void TryAim()
    {
        if (_playerTransform == null)
            return;

        ClearAiming();
        _attackState = AttackState.TryAiming;
        _attacker.RotateToIdle();
    }

    private void SetAim(Player player)
    {
        _playerTransform = player.transform;
        _playerCenterPosition = Vector3.up * player.GetComponent<CapsuleCollider>().height * _playerTransform.lossyScale.y / 2;

        TryAim();
    }

    private void Aim()
    {
        if (_cooldownTimer == 0)
        {
            AttackStarted?.Invoke();
            _animator.SetAiming(true);
            _attackState = AttackState.Aiming;
            _audioService.PlaySound(SoundType.ArcherStartAim);
            _lockTargetPosition = _playerTransform.position + _playerCenterPosition;
        }

        _attacker.RotateToAim(_lockTargetPosition);
        _cooldownTimer += Time.deltaTime;

        if (_cooldownTimer >= _aimingCooldown)
        {
            Shoot();
            _cooldownTimer = 0;
        }
    }

    private void StopTryAim()
    {
        ClearAiming();
        _attackState = AttackState.Idle;
        _attacker.ClearAim();
        _playerTransform = null;

        AttackStopped?.Invoke();
    }

    private void ClearAiming()
    {
        _lockTargetPosition = Vector3.zero;
        _cooldownTimer = 0;
        _animator.SetAiming(false);
    }

    private void Shoot()
    {
        Vector3 startPosition = _enemyTransform.position + _enemyCenterPosition;
        Vector3 targetPosition = _lockTargetPosition;
        Vector3 direction = (targetPosition - startPosition).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject arrow = _poolService.Get(ObjectPoolService.PoolObjectTypes.Arrow, startPosition, rotation);
        Rigidbody rigidbody = arrow.GetComponent<Rigidbody>();

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.AddForce(direction * _shootForce, ForceMode.Impulse);

        _attackState = AttackState.Shoot;
        _animator.SetRangedAttack();
        _audioService.PlaySound(SoundType.BowShot);
    }

    private bool CanSeePlayer()
    {
        if (_playerTransform == null)
            return false;

        Vector3 startPos = _enemyTransform.position + _enemyCenterPosition;
        Vector3 targetPos = _playerTransform.position + _playerCenterPosition;
        Vector3 directionToPlayer = targetPos - startPos;
        Vector3 lookDirection = _enemyTransform.forward;
        float angleToPlayer = Vector3.Angle(lookDirection, directionToPlayer);

        if (angleToPlayer > _viewAngle / 2f)
            return false;

        float distance = Vector3.Distance(startPos, targetPos);
        RaycastHit hit;

        Debug.DrawLine(startPos, targetPos, Color.red);

        return Physics.SphereCast(startPos, _checkHitRadius, directionToPlayer.normalized, out hit, distance, _obstacleLayer) == false;
    }
}
