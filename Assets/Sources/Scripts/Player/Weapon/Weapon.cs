using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Weapon : MonoBehaviour
{
    [SerializeField] private float _stickOffsetAngle = 50f;
    [SerializeField] private ParticleSystem _throwEffect;
    [SerializeField] private TrailRenderer _trailEffect;
    [SerializeField] private float _trailDuration = 0.1f;
    [SerializeField] private float _spinSpeed = 500f;

    private GameObject _gameObject;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private Transform _transform;
    private WeaponHandler _weaponHandler;
    private IAudioService _audioService;
    private WeaponRotator _weaponRotator;

    private Vector3 _startWeaponPosition;    
    private Quaternion _startWeaponRotation;
    
    private float _fixedZ = 0;    
    private float _throwForce = 15f;
    private float _bounceRotationForce = 1200f;
    private float _movementThreshold = 20f;
    private int _activeLayer;
    private int _passiveLayer;
    private bool _isThrown = false;
    private bool _isShouldRotate = false;
    private bool _isFirstHit;
    private bool _isIdle = true;
    private float _rotateAngle;
    private Coroutine _coroutine;

    public bool IsIdle => _isIdle;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();        
        
        _transform = transform;
        _gameObject = gameObject;

        _weaponRotator = new WeaponRotator(_transform, _stickOffsetAngle);

        _activeLayer = LayerMask.NameToLayer("PlayerWeaponActive");
        _passiveLayer = LayerMask.NameToLayer("PlayerWeaponPassive");
        _gameObject.layer = _passiveLayer;

        _rotateAngle = _spinSpeed;
        _bounceRotationForce = _spinSpeed > 0 ? _spinSpeed : _bounceRotationForce;
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

    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
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
            Ground ground = collision.collider.GetComponent<Ground>();
            ContactPoint hitPoint = collision.contacts[0];

            ResetEffects();

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
                Bounce(collision, shield.BounceForce, shield.BounceUpForce);
                return;
            }

            if (ground != null)
            {
                if (ground.BounceForce > 0 && _isFirstHit)
                {
                    Bounce(collision, ground.BounceForce);
                    _isFirstHit = false;
                }
                else if (ground.BounceForce == 0)
                {
                    ResetVelocity();
                    
                    _weaponRotator.RotateToObstacle(collision);
                    _rigidbody.isKinematic = true;
                    _transform.SetParent(ground.transform);
                }

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
        if (_weaponHandler == null || _transform == null)
            return;

        ResetEffects();

        _isThrown = false;
        _isShouldRotate = false;

        _transform.SetParent(_weaponHandler.transform);
        _transform.localPosition = _startWeaponPosition;
        _transform.localRotation = _startWeaponRotation;
                
        if (_rigidbody.isKinematic == false)
        {
            ResetVelocity();
            _rigidbody.isKinematic = true;
        }
    }

    public void Throw(Vector3 direction, float rotationAngle)
    {
        if (direction == Vector3.zero)
            throw new ArgumentNullException(nameof(direction));

        _weaponRotator.ResetRotation(rotationAngle);

        if (_spinSpeed == 0)
        {
            _weaponRotator.RotateBladeForward(direction);
        }

        _isThrown = true;
        _isShouldRotate = true;
        _isFirstHit = true;

        PerformEffects();
        
        _audioService.PlaySound(SoundType.ThrowWeapon);

        _rigidbody.isKinematic = false;
        _rigidbody.transform.SetParent(null);

        _rigidbody.AddForce(direction * _throwForce, ForceMode.Impulse);

        _rotateAngle = _spinSpeed;
    }

    public void SetActiveCollider(bool value)
    {
        if (_collider != null)
        _collider.enabled = value;
    }

    public void Activate()
    {
        _gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _gameObject.SetActive(false);
    }

    private void Bounce(Collision collision, float bounceForce, float upwardBounceForce = 0)
    {
        ContactPoint hitPoint = collision.contacts[0];
        Vector3 bounceDirection = hitPoint.normal;

        if (upwardBounceForce > 0)
        {
            bounceDirection.y = 0;
            bounceDirection.Normalize();
            ResetVelocity();
            bounceDirection += Vector3.up * (upwardBounceForce / bounceForce);
            bounceDirection.Normalize();
            _rigidbody.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
        }
        else
        {
            Vector3 incomingVelocity = collision.relativeVelocity * -1f;

            incomingVelocity.z = 0f;

            float incomingSpeed = incomingVelocity.magnitude;
            Vector3 reflectedDirection = Vector3.Reflect(incomingVelocity.normalized, bounceDirection).normalized;
            Vector3 weaponBounceForce = reflectedDirection * bounceForce;

            ResetVelocity();

            _rigidbody.AddForce(weaponBounceForce, ForceMode.Impulse);
        }        
        
        _rotateAngle = -_bounceRotationForce;
        _isShouldRotate = true;
    }

    private void ResetVelocity()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void PerformEffects()
    {
        if (_throwEffect != null)
            _throwEffect.Play(true);

        if (_trailEffect != null)
        {
            _trailEffect.emitting = true;
            _coroutine = StartCoroutine(ShowTrail());
        }
    }

    private IEnumerator ShowTrail()
    {
        yield return new WaitForSeconds(_trailDuration);

        _trailEffect.emitting = false;
    }

    private void ResetEffects()
    {
        if (_throwEffect != null)
        {
            _throwEffect.Clear();
            _throwEffect.Stop();
        }

        if (_trailEffect != null)
            _trailEffect.emitting = false;
    }
}
