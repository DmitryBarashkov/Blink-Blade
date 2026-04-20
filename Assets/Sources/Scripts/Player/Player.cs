using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RigBuilder))]
[RequireComponent(typeof(InputService))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    [SerializeField] Target _target;
    [SerializeField] Transform _weaponHandler;
    [SerializeField] private AimingArrow _aimingArrow;
    [SerializeField] private GroundChecker _groundChecker;    
    [SerializeField] private CinemachineVirtualCamera _camera;  

    private Transform _transform;
    private PlayerAnimator _animator;
    private RigBuilder _rigBuilder;
    private InputService _input;
    private Aimer _aimer;
    private Weapon _weapon;
    private Teleport _teleport;

    private bool _canTeleport = false;
    private bool _isAiming = false;

    private void Awake()
    {
        _transform = transform;
        
        _rigBuilder = GetComponent<RigBuilder>();
        _input = GetComponent<InputService>();

        _weapon = _weaponHandler.GetChild(0).GetComponent<Weapon>();

        _animator = new PlayerAnimator(GetComponent<Animator>());
        _teleport = new Teleport(_weapon, this);
        _aimer = new Aimer(_camera, _transform, _rigBuilder, _target, _weaponHandler, _weapon, _animator, _aimingArrow);
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
        _input.GetInput();

        if (_isAiming)
            _aimer.RotateToTarget();
    }

    private void OnAttackButtonUp()
    {
        _aimer.StopAim();
        _canTeleport = true;
        _isAiming = false;
    }

    private void OnAttackButtonPressed()
    {
        if (_canTeleport) 
        {
            _teleport.Perform();
            _canTeleport = false;
        }
        else
        {
            _aimer.StartAim();
            _isAiming = true;
        }        
    }

    private void OnGroundedChange(bool value)
    {
        _animator.SetGrounded(value);
    }
}
