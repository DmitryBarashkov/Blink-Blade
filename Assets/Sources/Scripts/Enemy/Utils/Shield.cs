using System;
using UnityEngine;

[RequireComponent(typeof(HitEffectSpawner))]
public class Shield : MonoBehaviour
{
    public event Action OnHitWeapon;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void HandleCollision(ContactPoint hitPoint)
    {
        OnHitWeapon?.Invoke();
    }

    internal void Activate()
    {
        _collider.enabled = true;
    }

    internal void Deactivate()
    {
        _collider.enabled = false;
    }
}
