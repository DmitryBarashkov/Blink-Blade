using System;
using BlinkBlade.Players;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace BlinkBlade.Enemies
{
    [RequireComponent(typeof(BoxCollider))]
    public class Blocker : MonoBehaviour
    {
        [Header("Щит")]
        [SerializeField] private Shield _shield;

        [Header("Компоненты Rigging")]
        [SerializeField] private MultiAimConstraint _bodyAimConstraint;
        [SerializeField] private Transform _aimTarget;

        [Header("Настройки")]
        [SerializeField] private float _responseSpeed = 8f;

        private Transform _playerWeaponTransform;

        private BoxCollider _collider;
        private ShieldEnemyAnimator _animator;
        private RigBuilder _rigBuilder;

        private bool _isWeaponInZone = false;

        public event Action StartBlocking;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();

            _bodyAimConstraint.weight = 0f;
        }

        private void Update()
        {
            float targetWeight = _isWeaponInZone ? 1f : 0f;

            _bodyAimConstraint.weight = Mathf.Lerp(_bodyAimConstraint.weight, targetWeight, Time.deltaTime * _responseSpeed);

            if (_isWeaponInZone && _playerWeaponTransform != null)
                _aimTarget.position = _playerWeaponTransform.position;
        }

        public void Initialize(Animator animator, RigBuilder rigBuilder)
        {
            _animator = new ShieldEnemyAnimator(animator);
            _rigBuilder = rigBuilder;
        }

        public void Activate()
        {
            Reset();
            StopBlock();

            _shield.WeaponHit += ShieldImpact;
            _shield.Activate();

            _rigBuilder.enabled = true;
            _collider.enabled = true;
        }

        public void Deactivate()
        {
            Reset();

            _shield.WeaponHit -= ShieldImpact;
            _shield.Deactivate();

            _rigBuilder.enabled = false;
            _collider.enabled = false;
        }

        public void StopBlock()
        {
            _animator.SetBlocking(false);
        }

        public void Reset()
        {
            _aimTarget.position = Vector3.zero;
            _isWeaponInZone = false;
        }

        private void StartBlock()
        {
            _animator.SetBlocking(true);
            StartBlocking?.Invoke();
        }

        private void ShieldImpact()
        {
            _animator.BlockImpact();
            StartBlocking?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            Weapon weapon = other.GetComponent<Weapon>();

            if (weapon != null)
            {
                _playerWeaponTransform = other.transform;
                _isWeaponInZone = true;
                StartBlock();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Weapon>() != null)
            {
                StopBlock();
                _isWeaponInZone = false;
            }
        }
    }
}