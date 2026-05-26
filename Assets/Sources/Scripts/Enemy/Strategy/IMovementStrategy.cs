using DG.Tweening;
using System;
using UnityEngine;

public interface IMovementStrategy
{
    event Action MovementStarted;
    void Initialize(Transform transform, EnemyAnimator animator, float wallCheckDistance, float cliffForwardOffset);
    void Activate();
    void Tick();
    void KeepMoving();
    void Deactivate();
    void Stop();
}

[Serializable]
public class Idle : IMovementStrategy
{
    public event Action MovementStarted = delegate { };
    public void Initialize(Transform transform, EnemyAnimator animator, float wallCheckDistance, float cliffForwardOffset) { }
    public void Activate() { }
    public void Tick() { }
    public void KeepMoving() { }
    public void Deactivate() { }
    public void Stop() { }
}

[Serializable]
public class Patrol : IMovementStrategy
{
    public event Action MovementStarted;
    
    private Transform _transform;
    private EnemyAnimator _animator;
    private LayerMask _groundLayer;

    private PatrolState _patrolState;

    private float _speed = 1.5f;
    private float _idleTime = 3f;
    private float _wallCheckDistance;
    private float _cliffForwardOffset;
    private float _cliffCheckDistance = 0.5f;
    private float _waitTimer = 0f;
    private float _rightTurn = 90f;
    private float _leftTurn = 270f;
    private float _turnSpeed = 0.4f;

    private bool _shouldRotate = true;
    private bool _isActive = true;

    public void Initialize(Transform transform, EnemyAnimator animator, float wallCheckDistance, float cliffForwardOffset)
    {
        _transform = transform;
        _animator = animator;
        _wallCheckDistance = wallCheckDistance;
        _cliffForwardOffset = cliffForwardOffset;
        _groundLayer = LayerMask.GetMask("Ground");
        _patrolState = PatrolState.Stopped;
    }

    public void Activate()
    {
        _patrolState = PatrolState.Moving;
        _isActive = true;
    }

    public void Deactivate()
    {
        _patrolState = PatrolState.Stopped;
        _animator.SetWalking(false);
        _isActive = false;
    }

    public void Tick()
    {
        if (_isActive)
        {
            switch(_patrolState)
            {
                case PatrolState.Stopped:
                    break;
                case PatrolState.Moving:
                    Move();
                    break;
                case PatrolState.Waiting:
                    Wait();
                    break;
                default:
                    break;
            }
        }
    }

    public void KeepMoving()
    {
        if (_isActive)
        {
            _waitTimer = 0f;
            _patrolState = PatrolState.Waiting;
            _shouldRotate = IsHittingWall() || IsAtCliff();
        }
    }

    public void Stop() 
    {
        _animator.SetWalking(false);
        _patrolState = PatrolState.Stopped;
    }

    private void Move()
    {
        MovementStarted?.Invoke();
        _animator.SetWalking(true);

        if (IsHittingWall() || IsAtCliff())
        {
            _patrolState = PatrolState.Waiting;
            _shouldRotate = true;
            return;
        }

        _transform.position += _transform.forward * _speed * Time.deltaTime;
        Utils.FixPositionZ(_transform);
    }

    private void Wait()
    {
        if (_patrolState == PatrolState.Rotating)
            return;

        _animator.SetWalking(false);
        _waitTimer += Time.deltaTime;

        if (_waitTimer >= _idleTime)
        {
            SwitchDirection();
        }
    }

    private void SwitchDirection()
    {
        if (_shouldRotate == false)
        {
            _patrolState = PatrolState.Moving;
            _waitTimer = 0f;
        }
        else
        {
            Vector3 currentRotation = _transform.eulerAngles;
            float targetTurn = currentRotation.y == _leftTurn ? _rightTurn : _leftTurn;

            _patrolState = PatrolState.Rotating;

            _transform.DORotate(new Vector3(currentRotation.x, targetTurn, currentRotation.z), _turnSpeed, RotateMode.Fast)
                .OnComplete(() =>
                {
                    _patrolState = PatrolState.Moving;
                    _waitTimer = 0f;
                });
        }
    }

    private bool IsAtCliff()
    {
        Vector3 origin = _transform.position + (_transform.forward * _cliffForwardOffset) + (Vector3.up * 0.1f);
        bool hasGroundAhead = Physics.Raycast(origin, Vector3.down, _cliffCheckDistance, _groundLayer);

        return !hasGroundAhead;
    }

    private bool IsHittingWall()
    {
        Vector3 origin = _transform.position + Vector3.up * 0.5f;

        return Physics.Raycast(origin, _transform.forward, _wallCheckDistance, _groundLayer);
    }

    public enum PatrolState
    {
        Waiting,
        Rotating,
        Moving,
        Stopped
    }
}
