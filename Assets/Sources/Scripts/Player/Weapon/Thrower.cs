using UnityEngine;

public class Thrower
{
    private Rigidbody _weapon;
    private Transform _weaponHandler;

    private float throwForce = 20f;
    private float torqueForce = 50f;

    public void Initialize(Rigidbody weapon, Transform weapohHandler)
    {
        _weapon = weapon;
        _weaponHandler = weapohHandler;
    }

    public void Throw(Vector3 direction)
    {
        if (_weapon != null )
        {
            _weapon.isKinematic = false;
            
            _weapon.AddForce(direction * throwForce, ForceMode.Impulse);
            //_weapon.AddTorque((_weaponHandler.forward + direction) * torqueForce, ForceMode.Force);
        }
    }
}
