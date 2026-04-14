using System;
using System.Collections;
using UnityEngine;

public class GroundChecker: MonoBehaviour
{
    private const float CheckDelay = 0.1f;

    [SerializeField] private LayerMask _layerMask;

    public event Action<bool> Grounded;

    private Transform _transform;
    private Coroutine _coroutine;

    private float _radius = 0.2f;
    
    private void Awake()
    {
        _transform = transform;
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
        while (enabled)
        {
            var wait = new WaitForSeconds(CheckDelay);

            Grounded?.Invoke(Physics.CheckSphere(_transform.position, _radius, _layerMask));

            yield return wait;
        }
    }
}
