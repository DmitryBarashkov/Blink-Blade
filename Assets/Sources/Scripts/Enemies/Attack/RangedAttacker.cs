using System;
using BlinkBlade.Game;
using BlinkBlade.Players;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace BlinkBlade.Enemies
{
    public class RangedAttacker : EnemyAttacker
    {
        private const float AimHeightFactor = 0.7f;
        private const float CheckHitRadius = 0.2f;

        [Header("Компоненты Rigging")]
        [SerializeField] private MultiAimConstraint _bodyAimConstraint;
        [SerializeField] private MultiAimConstraint _headAimConstraint;
        [SerializeField] private Transform _aimTarget;
        [SerializeField] private float _aimSpeed = 2f;
        [Header("Настройки атаки")]
        [SerializeField] private float _aimingCooldown = 1.2f;
        [SerializeField] private float _viewAngle = 120f;
        [SerializeField] private float _shootForce = 15f;

        private AttackState _attackState;
        private Transform _playerTransform;
        private Transform _enemyTransform;
        private Vector3 _playerCenterPosition;
        private Vector3 _enemyCenterPosition;
        private Vector3 _lockTargetPosition;
        private LayerMask _obstacleLayer;

        private enum AttackState
        {
            Idle,
            SearchAim,
            Aiming,
            Shoot,
        }

        private void Update()
        {
            if (_isActive == false || _playerTransform == null || _enemyTransform == null)
                return;

            if (_attackState != AttackState.Idle)
            {
                bool isSeePlayer = CanSeePlayer();

                if (isSeePlayer)
                    Aim();
                else
                    SearchAim();
            }
        }

        public override void Initialize(IAudioService audioService, EnemyAnimator animator, ObjectPoolService poolService, Enemy enemy)
        {
            base.Initialize(audioService, animator, poolService, enemy);

            CapsuleCollider enemyCollider = enemy.GetComponent<CapsuleCollider>();

            if (enemyCollider == null)
                throw new ArgumentNullException(nameof(enemyCollider));

            _enemyTransform = enemy.transform;
            _enemyCenterPosition = Vector3.up * enemyCollider.height * _enemyTransform.lossyScale.y * AimHeightFactor;
            _obstacleLayer = LayerMask.GetMask("Ground");

            ResetWeight();
        }

        public override void Activate()
        {
            _collider.enabled = true;
            ResetWeight();
            FindTarget();

            _isActive = true;
            _animator.SetAiming(false);
            _cooldownTimer = 0;
        }

        public override void Deactivate()
        {
            _collider.enabled = false;
            ResetWeight();

            _attackState = AttackState.Idle;

            _isActive = false;
        }

        public void RotateToAim(Vector3 target)
        {
            _aimTarget.position = target;

            _bodyAimConstraint.weight = Mathf.MoveTowards(_bodyAimConstraint.weight, 1f, _aimSpeed * Time.deltaTime);
            _headAimConstraint.weight = Mathf.MoveTowards(_bodyAimConstraint.weight, 1f, _aimSpeed * Time.deltaTime);
        }

        public void ClearAim()
        {
            ResetWeight();
        }

        protected override void Attack(Player player)
        {
            _playerTransform = player.transform;
            _playerCenterPosition = Vector3.up * player.GetComponent<CapsuleCollider>().height * _playerTransform.lossyScale.y / 2;

            SearchAim();
        }

        protected override void StopAttack()
        {
            ClearAiming();
            ClearAim();
            _attackState = AttackState.Idle;
            _playerTransform = null;

            InvokeAttackStopped();
        }

        private void ResetWeight()
        {
            _bodyAimConstraint.weight = 0;
            _headAimConstraint.weight = 0;
        }

        private void RotateToIdle()
        {
            _bodyAimConstraint.weight = Mathf.MoveTowards(_bodyAimConstraint.weight, 0f, _aimSpeed * Time.deltaTime);
            _headAimConstraint.weight = Mathf.MoveTowards(_bodyAimConstraint.weight, 0f, _aimSpeed * Time.deltaTime);
        }

        private void FindTarget()
        {
            CapsuleCollider capsuleCollider = _collider as CapsuleCollider;

            if (capsuleCollider == null)
                return;

            Vector3 worldCenter = capsuleCollider.transform.TransformPoint(capsuleCollider.center);

            Collider[] hitColliders = Physics.OverlapSphere(worldCenter, capsuleCollider.radius, _layerMask);

            if (hitColliders.Length > 0)
            {
                Player player = hitColliders[0].GetComponent<Player>();

                if (player != null)
                    Attack(player);
            }
        }

        private void ClearAiming()
        {
            _lockTargetPosition = Vector3.zero;
            _cooldownTimer = 0;
            _animator.SetAiming(false);
        }

        private void SearchAim()
        {
            if (_playerTransform == null)
                return;

            ClearAiming();
            _attackState = AttackState.SearchAim;
            RotateToIdle();
        }

        private void Aim()
        {
            if (_cooldownTimer == 0)
            {
                InvokeAttackStarted();

                _animator.SetAiming(true);
                _attackState = AttackState.Aiming;
                _audioService.PlaySound(SoundType.ArcherStartAim);
                _lockTargetPosition = _playerTransform.position + _playerCenterPosition;
            }

            RotateToAim(_lockTargetPosition);
            _cooldownTimer += Time.deltaTime;

            if (_cooldownTimer >= _aimingCooldown)
            {
                Shoot();
                _cooldownTimer = 0;
            }
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

            return Physics.SphereCast(startPos, CheckHitRadius, directionToPlayer.normalized, out hit, distance, _obstacleLayer) == false;
        }
    }
}