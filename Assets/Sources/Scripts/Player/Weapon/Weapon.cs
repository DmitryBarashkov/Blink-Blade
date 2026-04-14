using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Transform _transform;
    private Transform _weaponHandler;

    private Vector3 _startWeaponPosition;
    private Quaternion _startWeaponRotation;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
        _weaponHandler = transform.parent;

        _startWeaponPosition = _transform.localPosition;
        _startWeaponRotation = _transform.localRotation;
    }

    private void ChangePositionToObstacle(Collision collision)
    {
        Debug.Log($"Collided with {collision.collider.name}");

        _rigidbody.isKinematic = true;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        Vector3 impactDir = collision.relativeVelocity.normalized;

        _transform.forward = impactDir;
        _transform.position += _transform.forward * 0.2f;
        _transform.SetParent(collision.transform);
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

    private void OnCollisionEnter(Collision collision)
    {
        //ChangePositionToObstacle(collision);
    }

    private void OnDrawGizmosSelected()
    {
        if (_rigidbody != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.TransformPoint(_rigidbody.centerOfMass), 0.05f);
        }
    }
}
