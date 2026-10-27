using System;
using System.Collections;
using BlinkBlade.Game;
using UnityEngine;
using Zenject;

namespace BlinkBlade.Props
{
    public class ArrowTrap : MonoBehaviour
    {
        [SerializeField] private float _cooldown = 2f;

        private ObjectPoolService _poolService;
        private IAudioService _audioService;
        private Transform _transform;
        private Coroutine _coroutine;

        private float _shootForce = 15f;
        private float _offset = 0.5f;

        private bool _isActive;

        [Inject]
        public void Construct(ObjectPoolService poolService, IAudioService audioService)
        {
            _poolService = poolService;
            _audioService = audioService;
        }

        private void Awake()
        {
            _transform = transform;
        }

        public void Activate()
        {
            _isActive = true;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(WaitCooldownForShot());
        }

        public void Deactivate()
        {
            _isActive = false;

            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }

        private IEnumerator WaitCooldownForShot()
        {
            var waitForCooldown = new WaitForSeconds(_cooldown);

            while (_isActive)
            {
                yield return waitForCooldown;

                Shoot();
            }
        }

        private void Shoot()
        {
            _audioService.PlaySound(SoundType.BowShot);

            Vector3 arrowStartPosition = _transform.position + (_transform.forward * _offset);
            GameObject arrow = _poolService.Get(ObjectPoolService.PoolObjectTypes.Arrow, arrowStartPosition, _transform.rotation);

            if (arrow == null)
                throw new ArgumentNullException(nameof(arrow));

            Rigidbody rigidbody = arrow.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                rigidbody.AddForce(_transform.forward * _shootForce, ForceMode.Impulse);
            }
        }
    }
}