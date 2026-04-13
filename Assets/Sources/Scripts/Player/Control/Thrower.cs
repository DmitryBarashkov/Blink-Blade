using UnityEngine;

public class Thrower
{
    private Rigidbody _weaponRigidbody;    

    private float throwForce = 20f;
    private float torqueForce = 50f;

    public Thrower(Rigidbody weaponRigidbody)
    {
        _weaponRigidbody = weaponRigidbody;        
    }

    public void Throw(Vector3 direction)
    {
        if (_weaponRigidbody != null )
        {
            _weaponRigidbody.isKinematic = false;
            _weaponRigidbody.transform.SetParent(null);
            
            _weaponRigidbody.AddForce(direction * throwForce, ForceMode.Impulse);            
            _weaponRigidbody.AddRelativeTorque(-Vector3.forward * torqueForce, ForceMode.Impulse);
        }
    }
}
