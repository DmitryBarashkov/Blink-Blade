using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ParticleSystem))]
public class Weapon : MonoBehaviour
{
    private ParticleSystem _throwEffect;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private WeaponHandler _weaponHandler;    

    private Vector3 _startWeaponPosition;    
    private Quaternion _startWeaponRotation;

    private float _fixedZ = 0;
    private float _spinSpeed = 500f;
    private float _throwForce = 15f;    
    private bool _isThrown = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _throwEffect = GetComponentInChildren<ParticleSystem>();
        _transform = transform;        
    }

    private void Update()
    {
        if (_isThrown)
        {
            _transform.Rotate(0, 0, _spinSpeed * Time.deltaTime, Space.Self);
        }
    }

    private void LateUpdate()
    {
        if (transform.position.z != _fixedZ && _rigidbody.isKinematic == false)
            Utils.FixPositionZ(_transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isThrown = false;
            
        Enemy enemy = collision.collider.GetComponent<Enemy>();        

        if (enemy != null)
        {
            enemy.Die(collision.contacts[0]);
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

    public void Initialize(WeaponHandler weaponHandler)
    {
        _weaponHandler = weaponHandler;        
        
        _transform.SetParent(_weaponHandler.transform);
        _transform.localPosition = _startWeaponPosition = _transform.position;
        _transform.localRotation = _startWeaponRotation = _transform.rotation;        
    }

    public void ReturnToWeaponHandler()
    {
        if (_weaponHandler == null)
            return;

        _isThrown = false;        

        _transform.SetParent(_weaponHandler.transform);
        _transform.localPosition = _startWeaponPosition;
        _transform.localRotation = _startWeaponRotation;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.isKinematic = true;
    }

    public void Throw(Vector3 direction, float rotationAngle)
    {
        if (direction == Vector3.zero)
            throw new ArgumentNullException(nameof(direction));

        ResetRotation(rotationAngle);

        _isThrown = true;
        _throwEffect.Play();

        _rigidbody.isKinematic = false;
        _rigidbody.transform.SetParent(null);

        _rigidbody.AddForce(direction * _throwForce, ForceMode.Impulse);        
    }

    private void ResetRotation(float rotationAngle)
    {
        if (rotationAngle == 0)
            throw new ArgumentNullException(nameof(rotationAngle));

        if (rotationAngle > 0)
        {
            _transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            _transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
