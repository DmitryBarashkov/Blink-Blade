using System;
using UnityEngine;
using YG;
using Zenject;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HitEffectSpawner))]
public class Player : MonoBehaviour, IResetable
{
    public event Action Dead;

    [SerializeField] private ParticleSystem _teleportEffect;
    [SerializeField] private WeaponHandler _weaponHandler;
    [SerializeField] private GroundChecker _groundChecker;

    [Inject] private PlayerStats _playerStats;
    [Inject] private Level _level;
    
    private Transform _transform;
    private Transform _spawnPoint;    
        
    private PlayerAnimator _animator;
    private Rigidbody _rigidBody;
    private CapsuleCollider _collider;
    private HitEffectSpawner _effect;
    
    private InputService _input;
    private IAudioService _audioService;
    
    private Aimer _aimer;
    private Teleport _teleport;
    
    private PlayerWeaponController _weaponController;

    private bool _canTeleport = false;
    private bool _isAiming = false;
    private bool _isDead = false;
    private bool _isInvincible;

    private int _energy;

    public bool IsInvincible => _isInvincible;

    private void Awake()
    {
        _animator = new PlayerAnimator(GetComponent<Animator>());
        _rigidBody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _effect = GetComponent<HitEffectSpawner>();

        _weaponController.Initialize(_weaponHandler);
        _teleport.Initialize(_weaponController.CurrentWeapon, _transform, _collider, _rigidBody, _teleportEffect);
        _aimer.Initialize(_transform, _animator);

        _playerStats.currentEnergy.Value = _energy;
    }

    private void OnEnable()
    {
        _input.AttackBtnPressed += OnAttackButtonPressed;
        _input.AttackBtnUp += OnAttackButtonUp;
        _groundChecker.Grounded += OnGroundedChange;
    }

    private void OnDisable()
    {
        _input.AttackBtnPressed -= OnAttackButtonPressed;
        _input.AttackBtnUp -= OnAttackButtonUp;
        _groundChecker.Grounded -= OnGroundedChange;
    }

    private void Update()
    {
        if (_input != null)
            _input.GetInput();

        if (_isAiming)
            _aimer.RotateToTarget();        
    }

    [Inject]
    private void Construct(InputService input, PlayerWeaponController weaponController, Teleport teleport, Aimer aimer, 
                           int energy, Transform spawnPoint, IAudioService audioService)
    {
        _input = input;        
        _weaponController = weaponController;
        _teleport = teleport;
        _aimer = aimer;
        _energy = energy;
        _spawnPoint = spawnPoint;
        _transform = transform;

        _audioService = audioService;
        _transform.position = spawnPoint.position;
        _transform.rotation = spawnPoint.rotation;
    }

    public void AddEnergy(int addCount)
    {
        _energy += addCount;
        _playerStats.currentEnergy.Value = _energy;
    }

    public void ResetOnRestart()
    {
        _transform.position = _spawnPoint.position;
        _transform.rotation = _spawnPoint.rotation;        
        
        _canTeleport = false;
        _isAiming = false;
        _isDead = false;
        _isInvincible = false;

        _energy = YG2.saves.energy;
        _playerStats.currentEnergy.Value = _energy;
        
        _rigidBody.useGravity = false;
        _rigidBody.isKinematic = false;
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
        
        _collider.enabled = true;
        _animator.SetDied(false);
        _groundChecker.gameObject.SetActive(true);

        _weaponController.ActivateWeapon();
    }

    public void Die(ContactPoint hitPoint)
    {
        Dead?.Invoke();

        _input.Deactivate();
        
        _effect.Perform(hitPoint);
        _audioService.PlaySound(SoundType.Hurt);

        _aimer.StopAim(false);
        _aimer.SetCameraAim(_transform);
        _weaponController.DeactivateWeapon();

        _rigidBody.useGravity = true;
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
        _isInvincible = true;

        _isDead = true;
        _canTeleport = false;
        _isAiming = false;
    }

    private void OnAttackButtonUp()
    {
        if (_isAiming)
        {
            _aimer.StopAim();
            _canTeleport = true;
            _isAiming = false;
        }
    }

    private void OnAttackButtonPressed()
    {
        if (_canTeleport) 
        {
            _energy--;
            _playerStats.currentEnergy.Value = _energy;
            _canTeleport = false;
            _teleport.Perform();
        }
        else if (_energy == 0)
        {
            _aimer.SetCameraAim(_transform);
            _isAiming = false;
            Defeat(true);
        }
        else
        {
            _aimer.StartAim();
            _isAiming = true;
        }              
    }

    private void Defeat(bool isOutOfEnergy = false)
    {
        _level.Lose(isOutOfEnergy);
    }

    private void OnGroundedChange(bool value)
    {
        _animator.SetGrounded(value);

        if (_isDead && value)
        {
            _groundChecker.gameObject.SetActive(false);
            _rigidBody.useGravity = false;
            _rigidBody.isKinematic = true;
            _collider.enabled = false;
            _animator.SetDied(true);
            _audioService.PlaySound(SoundType.FallingOnGround);
            
            Defeat();
        }
    }
}
