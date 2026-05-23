using DG.Tweening;
using System;
using UnityEngine;

public interface IMovementStrategy
{
    void Initialize(Transform transform, EnemyAnimator animator, float wallCheckDistance);
    void Start();
    void Tick();
    void KeepMoving();
    void Stop();
}

[Serializable]
public class Idle : IMovementStrategy
{
    public void Initialize(Transform transform, EnemyAnimator animator, float wallCheckDistance) { }
    public void Start() { }
    public void Tick() { }
    public void KeepMoving() { }
    public void Stop() { }
}

[Serializable]
public class Patrol : IMovementStrategy
{
    private Transform _transform;
    private EnemyAnimator _animator;
    private LayerMask _groundLayer;

    private PatrolState _patrolState;

    private float _speed = 1.5f;
    private float _idleTime = 3f;
    private float _wallCheckDistance;
    private float _cliffForwardOffset = 0.4f;
    private float _cliffCheckDistance = 0.5f;
    private float _waitTimer = 0f;
    private float _rightTurn = 90f;
    private float _leftTurn = 270f;
    private float _turnSpeed = 0.4f;

    public void Initialize(Transform transform, EnemyAnimator animator, float wallCheckDistance)
    {
        _transform = transform;
        _animator = animator;
        _wallCheckDistance = wallCheckDistance;
        _groundLayer = LayerMask.GetMask("Ground");
        _patrolState = PatrolState.Stopped;
    }

    public void Start()
    {
        _patrolState = PatrolState.Moving;
    }

    public void Stop()
    {
        _patrolState = PatrolState.Stopped;
        _animator.SetWalking(false);
    }

    public void Tick()
    {
        if (_patrolState == PatrolState.Stopped)
            return;

        if (_patrolState == PatrolState.Moving)
            Move();
        else if (_patrolState == PatrolState.Waiting)
            Wait();
    }

    public void KeepMoving()
    {
        _patrolState = PatrolState.Waiting;
    }

    private void Move()
    {
        _animator.SetWalking(true);

        if (IsHittingWall() || IsAtCliff())
        {
            _patrolState = PatrolState.Waiting;
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
