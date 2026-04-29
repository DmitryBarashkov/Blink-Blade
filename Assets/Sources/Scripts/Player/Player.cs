using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using YG;
using Zenject;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RigBuilder))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    [Inject] private PlayerStats _playerStats;
    
    private Transform _transform;
        
    private PlayerAnimator _animator;
    
    private RigBuilder _rigBuilder;
    private InputService _input;
    
    private Aimer _aimer;
    private Teleport _teleport;
    
    private Weapon _weapon;
    private WeaponHandler _weaponHandler;
    
    private GroundChecker _groundChecker;       

    private bool _canTeleport = false;
    private bool _isAiming = false;

    private int _energy;
    private int _coins;

    private void Awake()
    {
        _transform = transform;
        
        _rigBuilder = GetComponent<RigBuilder>();            

        _animator = new PlayerAnimator(GetComponent<Animator>());
        
        _weapon.Initialize(_weaponHandler);
        _teleport.Initialize(_weapon, this);
        _aimer.Initialize(_transform, _rigBuilder, _animator, _weaponHandler);

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
    private void Construct(InputService input, Weapon weapon, Teleport teleport, Aimer aimer, int energy, int coins)
    {
        _input = input;        
        _weapon = weapon;        
        _teleport = teleport;
        _aimer = aimer;
        _energy = energy;
        _coins = coins;

        _weaponHandler = GetComponentInChildren<WeaponHandler>();
        _groundChecker = GetComponentInChildren<GroundChecker>();
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
        if (_energy == 0)
            Defeat();
        
        if (_canTeleport) 
        {
            _teleport.Perform();
            _energy--;
            _playerStats.currentEnergy.Value = _energy;            
            _canTeleport = false;
        }
        else
        {
            _aimer.StartAim();
            _isAiming = true;
        }        
    }

    private void Defeat()
    {
        Debug.Log("Энергия закончилась");
    }

    private void OnGroundedChange(bool value)
    {
        _animator.SetGrounded(value);
    }
}
