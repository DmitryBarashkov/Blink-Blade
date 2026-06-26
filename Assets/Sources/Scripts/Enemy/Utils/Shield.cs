using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HitEffectSpawner))]
public class Shield : MonoBehaviour
{
    public event Action OnHitWeapon;

    private Collider _collider;
    private Coroutine _coroutine;

    private float _cooldown = 0.2f;
    private bool _isActive;

    private float _bounceForce = 5f;
    private float _bounceUpForce = 3f;

    public float BounceForce => _bounceForce;
    public float BounceUpForce=>  _bounceUpForce;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    public void HandleCollision(ContactPoint hitPoint)
    {
        OnHitWeapon?.Invoke();

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ShieldCooldown());
    }

    public void Activate()
    {
        _collider.enabled = true;
        _isActive = true;
    }

    public void Deactivate()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _collider.enabled = false;
        _isActive = false;
    }

    private IEnumerator ShieldCooldown()
    {
        _collider.enabled = false;

        yield return new WaitForSeconds(_cooldown);

        if (_isActive)
            _collider.enabled = true;
    }
}
