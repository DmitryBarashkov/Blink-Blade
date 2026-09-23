using System;
using System.Collections;
using UnityEngine;

namespace BlinkBlade.Players
{
    public class GroundChecker : MonoBehaviour
    {
        private const float CheckDelay = 0.1f;

        [SerializeField] private float _radius = 0.2f;

        private Transform _transform;
        private Coroutine _coroutine;
        private LayerMask _layerMask;

        public event Action<bool> Grounded;

        private void Awake()
        {
            _transform = transform;
            _layerMask = LayerMask.GetMask("Ground");
        }

        private void OnEnable()
        {
            _coroutine = StartCoroutine(IsGrounded());
        }

        private void OnDisable()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }

        private IEnumerator IsGrounded()
        {
            var wait = new WaitForSeconds(CheckDelay);

            while (enabled)
            {
                Grounded?.Invoke(Physics.CheckSphere(_transform.position, _radius, _layerMask));

                yield return wait;
            }
        }
    }
}