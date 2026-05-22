using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ParticleSystem))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private float _rotationOffsetAngle;

    private GameObject _gameObject;
    private ParticleSystem _throwEffect;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private Transform _transform;
    private WeaponHandler _weaponHandler;
    private IAudioService _audioService;
    private WeaponRotator _weaponRotator;

    private Vector3 _startWeaponPosition;    
    private Quaternion _startWeaponRotation;
    
    private float _fixedZ = 0;
    private float _spinSpeed = 500f;
    private float _throwForce = 15f;
    private float _upwardBounceForce = 3f;
    private float _bounceForce = 5f;
    private float _movementThreshold = 20f;
    private int _activeLayer;
    private int _passiveLayer;
    private bool _isThrown = false;
    private bool _isShouldRotate = false;
    private bool _isIdle = true;
    private float _rotateAngle;

    public bool IsIdle => _isIdle;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _throwEffect = GetComponentInChildren<ParticleSystem>();
        
        _transform = transform;
        _gameObject = gameObject;

        _weaponRotator = new WeaponRotator(_transform, _rotationOffsetAngle);

        _activeLayer = LayerMask.NameToLayer("PlayerWeaponActive");
        _passiveLayer = LayerMask.NameToLayer("PlayerWeaponPassive");
        _gameObject.layer = _passiveLayer;

        _rotateAngle = _spinSpeed;
    }

    private void Update()
    {
        if (_isShouldRotate)
        {
            _transform.Rotate(0, 0, _rotateAngle * Time.deltaTime, Space.Self);
        }

        _isIdle = _rigidbody.velocity.sqrMagnitude < _movementThreshold;

        if (_isIdle)
            _gameObject.layer = _passiveLayer;
        else
            _gameObject.layer = _activeLayer;
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

        if (_isThrown && IsIdle == false)
        {
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            Shield shield = collision.collider.GetComponent<Shield>();
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

            if (shield != null)
            {
                shield.HandleCollision(hitPoint);
                Bounce(hitPoint);
                return;
            }
        }

        _isShouldRotate = false;
    }

    public void Initialize(WeaponHandler weaponHandler, IAudioService audioService)
    {
        _weaponHandler = weaponHandler;
        _audioService = audioService;
        
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
        _audioService.PlaySound(SoundType.ThrowWeapon);

        _rigidbody.isKinematic = false;
        _rigidbody.transform.SetParent(null);

        _rigidbody.AddForce(direction * _throwForce, ForceMode.Impulse);

        _rotateAngle = _spinSpeed;
    }

    public void SetActive(bool value)
    {
        _collider.enabled = value;
    }

    private void Bounce(ContactPoint hitPoint)
    {
        Vector3 bounceDirection = hitPoint.normal;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        bounceDirection.y = 0;
        bounceDirection.Normalize();
        bounceDirection += Vector3.up * (_upwardBounceForce / _bounceForce);
        bounceDirection.Normalize();

        _rigidbody.AddForce(bounceDirection * _bounceForce, ForceMode.Impulse);
        
        _rotateAngle = -_spinSpeed;
        _isShouldRotate = true;
    }
}
