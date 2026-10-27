using System;
using BlinkBlade.Common;
using BlinkBlade.Game;
using DG.Tweening;
using UnityEngine;

namespace BlinkBlade.Enemies
{
    [Serializable]
    public class Patrol : Mover
    {
        private const float RightTurn = 90f;
        private const float LeftTurn = 270f;

        private LayerMask _groundLayer;

        private PatrolState _patrolState;

        [SerializeField] private float _speed = 1.5f;
        [SerializeField] private float _idleTime = 3f;
        [SerializeField] private float _wallCheckDistance = 1f;
        [SerializeField] private float _cliffForwardOffset = 0.5f;
        [SerializeField] private float _cliffCheckDistance = 0.5f;
        [SerializeField] private float _turnSpeed = 0.4f;

        private float _waitTimer = 0f;

        private bool _shouldRotate = true;

        public enum PatrolState
        {
            Waiting,
            Rotating,
            Moving,
            Stopped,
        }

        private void Update()
        {
            if (_isActive)
            {
                switch (_patrolState)
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

        public override void Initialize(
            Transform transform,
            CapsuleCollider collider,
            EnemyAnimator animator,
            ILevelData levelData,
            IAudioService audioService)
        {
            base.Initialize(transform, collider, animator, levelData, audioService);

            _groundLayer = LayerMask.GetMask("Ground");
            _patrolState = PatrolState.Stopped;
        }

        public override void Activate()
        {
            _patrolState = PatrolState.Moving;
            _isActive = true;
        }

        public override void Deactivate()
        {
            _patrolState = PatrolState.Stopped;
            _animator.SetWalking(false);
            _isActive = false;
        }

        public override void KeepMoving()
        {
            _isActive = true;
            _waitTimer = 0f;
            _patrolState = PatrolState.Waiting;
            _shouldRotate = IsHittingWall() || IsAtCliff();
        }

        public override void Stop()
        {
            _animator.SetWalking(false);
            _patrolState = PatrolState.Stopped;
        }

        public override void Move()
        {
            InvokeMovementStarted();
            _animator.SetWalking(true);

            if (IsHittingWall() || IsAtCliff())
            {
                _patrolState = PatrolState.Waiting;
                _shouldRotate = true;
                return;
            }

            _transform.position += _transform.forward * _speed * Time.deltaTime;
            CommonFunctions.FixPositionZ(_transform);
        }

        private void Wait()
        {
            if (_patrolState == PatrolState.Rotating)
                return;

            _animator.SetWalking(false);
            _waitTimer += Time.deltaTime;

            if (_waitTimer >= _idleTime)
                SwitchDirection();
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
                float targetTurn = currentRotation.y == LeftTurn ? RightTurn : LeftTurn;

                _patrolState = PatrolState.Rotating;

                _transform.DORotate(new Vector3(currentRotation.x, targetTurn, currentRotation.z), _turnSpeed, RotateMode.Fast)
                    .OnComplete(() =>
                    {
                        if (_patrolState != PatrolState.Stopped)
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
            Vector3 origin = _transform.position + (Vector3.up * 0.5f);

            return Physics.Raycast(origin, _transform.forward, _wallCheckDistance, _groundLayer);
        }
    }
}