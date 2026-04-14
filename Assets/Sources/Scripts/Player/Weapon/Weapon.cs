using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Weapon : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Transform _transform;
    private Transform _weaponHandler;

    private Vector3 _startWeaponPosition;
    private Quaternion _startWeaponRotation;

    private float _fixedZ = 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
        _weaponHandler = transform.parent;

        _startWeaponPosition = _transform.localPosition;
        _startWeaponRotation = _transform.localRotation;
    }

    private void LateUpdate()
    {
        if (transform.position.z != _fixedZ && _rigidbody.isKinematic == false)
            Utils.FixPositionZ(_transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.collider.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.Die();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_rigidbody != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.TransformPoint(_rigidbody.centerOfMass), 0.05f);
        }
    }

    public void ReturnToWeaponHandler()
    {
        if (_weaponHandler == null)
            return;
        
        _transform.SetParent(_weaponHandler);
        _transform.localPosition = _startWeaponPosition;
        _transform.localRotation = _startWeaponRotation;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;
    }
}
