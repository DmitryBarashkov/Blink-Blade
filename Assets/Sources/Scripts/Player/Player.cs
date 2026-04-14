using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RigBuilder))]
[RequireComponent(typeof(InputService))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    [SerializeField] SmartFocusCamera _focusCamera;
    [SerializeField] Transform _targetTransform;
    [SerializeField] Transform _weaponHandler;
    [SerializeField] private GameObject _aimingArrow;

    private Transform _transform;
    private Animator _animator;
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
        
        _animator = GetComponent<Animator>();
        _rigBuilder = GetComponent<RigBuilder>();
        _input = GetComponent<InputService>();        

        _weapon = _weaponHandler.GetChild(0).GetComponent<Weapon>();
        
        _teleport = new Teleport(_weapon, _transform, GetComponent<CapsuleCollider>(), _focusCamera);
        _aimer = new Aimer(_transform, _focusCamera, _rigBuilder, _targetTransform, _weaponHandler, _weapon, _animator, _aimingArrow);
    }

    private void OnEnable()
    {
        _input.AttackBtnPressed += OnAttackButtonPressed;
        _input.AttackBtnUp += OnAttackButtonUp;
    }

    private void OnDisable()
    {
        _input.AttackBtnPressed -= OnAttackButtonPressed;
        _input.AttackBtnUp -= OnAttackButtonUp;
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
}
