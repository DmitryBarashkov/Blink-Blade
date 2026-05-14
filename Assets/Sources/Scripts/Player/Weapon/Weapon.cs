using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ParticleSystem))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private float _rotationOffsetAngle;
    
    private ParticleSystem _throwEffect;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private WeaponHandler _weaponHandler;
    private WeaponRotator _weaponRotator;

    private Vector3 _startWeaponPosition;    
    private Quaternion _startWeaponRotation;

    private float _fixedZ = 0;
    private float _spinSpeed = 500f;
    private float _throwForce = 15f;    
    private bool _isThrown = false;
    private bool _isShouldRotate = false;    

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _throwEffect = GetComponentInChildren<ParticleSystem>();
        _transform = transform;
        _weaponRotator = new WeaponRotator(_transform, _rotationOffsetAngle);
    }

    private void Update()
    {
        if (_isShouldRotate)
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
        if (_isShouldRotate)
            _weaponRotator.RotateToObstacle(collision);

        _isShouldRotate = false;

        if (_isThrown)
        {
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            HitEffectSpawner effect = collision.collider.GetComponent<HitEffectSpawner>();
            ContactPoint hitPoint = collision.contacts[0];

            if (effect != null)
            {
                effect.Perform(hitPoint);
            }

            if (enemy != null)
            {
                enemy.Die(hitPoint);             
            }            
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
        _isShouldRotate = false;

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

        _weaponRotator.ResetRotation(rotationAngle);

        _isThrown = true;
        _isShouldRotate = true;
        _throwEffect.Play();

        _rigidbody.isKinematic = false;
        _rigidbody.transform.SetParent(null);

        _rigidbody.AddForce(direction * _throwForce, ForceMode.Impulse);        
    }
}
