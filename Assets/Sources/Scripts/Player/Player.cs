using System;
using UnityEditor;
using UnityEngine;
using YG;
using Zenject;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HitEffectSpawner))]
public class Player : MonoBehaviour
{
    [SerializeField] private ParticleSystem _teleportEffect;
    [SerializeField] private WeaponHandler _weaponHandler;
    [SerializeField] private GroundChecker _groundChecker;

    [Inject] private PlayerStats _playerStats;
    [Inject] private Level _level;

    private Transform _transform;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private GameObject _gameObject;

    private PlayerAnimator _animator;
    private Rigidbody _rigidBody;
    private CapsuleCollider _collider;
    private HitEffectSpawner _effect;
    private int _alivelayerMask;
    private int _invincibleMask;

    private InputService _input;
    private IAudioService _audioService;
    private Aimer _aimer;
    private Teleport _teleport;

    private PlayerWeaponController _weaponController;

    private bool _canTeleport = false;
    private bool _isAiming = false;
    private bool _isDead = false;
    private bool _isInvincible;

    public event Action Dead;

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

        _gameObject = gameObject;

        _alivelayerMask = LayerMask.NameToLayer("Player");
        _invincibleMask = LayerMask.NameToLayer("InvinciblePlayer");
    }

    private void OnEnable()
    {
        _input.AttackBtnPressed += OnAttackButtonPressed;
        _input.AttackBtnUp += OnAttackButtonUp;
        _input.MenuOpenBtnPressed += OnMenuOpenBtnPressed;
        _groundChecker.Grounded += OnGroundedChange;
        _level.LevelFinished += OnLevelFinished;
    }

    private void OnDisable()
    {
        _input.AttackBtnPressed -= OnAttackButtonPressed;
        _input.AttackBtnUp -= OnAttackButtonUp;
        _input.MenuOpenBtnPressed -= OnMenuOpenBtnPressed;
        _groundChecker.Grounded -= OnGroundedChange;
        _level.LevelFinished -= OnLevelFinished;
    }

    private void Update()
    {
        if (_input != null)
            _input.GetInput();

        if (_isAiming)
        {
            _aimer.PerformAim();
        }
    }

    [Inject]
    public void Construct(InputService input, PlayerWeaponController weaponController, Teleport teleport, Aimer aimer, IAudioService audioService)
    {
        _input = input;
        _weaponController = weaponController;
        _teleport = teleport;
        _aimer = aimer;

        _audioService = audioService;

        _transform = transform;
        _initialPosition = _transform.position;
        _initialRotation = _transform.rotation;
    }

    public void Initialize(Vector3 position, Quaternion rotation)
    {
        _initialPosition = position;
        _initialRotation = rotation;

        _transform.position = _initialPosition;
        _transform.rotation = _initialRotation;

        _playerStats.CurrentEnergy.Value = YG2.saves.Energy;

        _canTeleport = false;
        _isAiming = false;
        _isDead = false;
        _isInvincible = false;

        _gameObject.layer = _alivelayerMask;

        SetInvincibility(false);

        _rigidBody.isKinematic = false;
        _rigidBody.useGravity = false;

        _collider.enabled = true;
        _animator.SetDied(false);
        _groundChecker.gameObject.SetActive(true);
        _aimer.Initialize(_transform, _animator);

        _weaponController.ActivateWeapon();
    }

    public void Die(ContactPoint hitPoint)
    {
        Dead?.Invoke();

        _input.Deactivate();

        if (EditorPrefs.GetBool("EnabledBlood"))
            _effect.Perform(hitPoint);

        _audioService.PlaySound(SoundType.Hurt);

        _weaponController.DeactivateWeapon();

        SetInvincibility(true);
        ResetVelocity();

        _rigidBody.useGravity = true;

        _isDead = true;
        _canTeleport = false;

        Defeat();
        StopAim();
    }

    public void Activate()
    {
        _collider.enabled = true;

        StopAim();
        SetInvincibility(false);
    }

    private void SetInvincibility(bool value)
    {
        _isInvincible = value;
        _gameObject.layer = _isInvincible ? _invincibleMask : _alivelayerMask;
    }

    private void StopAim()
    {
        _aimer.StopAim(false);
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
            _playerStats.CurrentEnergy.Value--;
            _canTeleport = false;
            _teleport.Perform();
        }
        else if (_playerStats.CurrentEnergy.Value == 0)
        {
            StopAim();
            Defeat(true);
        }
        else
        {
            _aimer.StartAim();
            _isAiming = true;
        }
    }

    private void OnMenuOpenBtnPressed()
    {
        if (_aimer.CanShowMenu)
        {
            _isAiming = false;
            _aimer.StopAim(false);
            _level.ShowMenu();
        }
    }

    private void Defeat(bool isOutOfEnergy = false)
    {
        _level.Lose(isOutOfEnergy);

        if (isOutOfEnergy)
        {
            _collider.enabled = false;
            SetInvincibility(true);
        }
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
        }
    }

    private void ResetVelocity()
    {
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
    }

    private void OnLevelFinished()
    {
        SetInvincibility(true);
    }

    public class Factory : PlaceholderFactory<UnityEngine.Object, Player>
    {
    }
}
