using System;
using UnityEngine;
using YG;
using Zenject;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour, IResetable
{
    [Inject] private PlayerStats _playerStats;
    [Inject] private Level _level;
    
    private Transform _transform;
    private Transform _spawnPoint;    
        
    private PlayerAnimator _animator;
    
    private InputService _input;
    
    private Aimer _aimer;
    private Teleport _teleport;
    
    private Weapon _weapon;
    private WeaponHandler _weaponHandler;
    
    private GroundChecker _groundChecker;       

    private bool _canTeleport = false;
    private bool _isAiming = false;

    private int _energy;    

    private void Awake()
    {
        _transform = transform;
        
        _animator = new PlayerAnimator(GetComponent<Animator>());        

        _weapon.Initialize(_weaponHandler);
        _teleport.Initialize(_weapon, this);
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
    private void Construct(InputService input, Weapon weapon, Teleport teleport, Aimer aimer, int energy, Transform spawnPoint)
    {
        _input = input;        
        _weapon = weapon;        
        _teleport = teleport;
        _aimer = aimer;
        _energy = energy;
        _spawnPoint = spawnPoint;

        _weaponHandler = GetComponentInChildren<WeaponHandler>();
        _groundChecker = GetComponentInChildren<GroundChecker>();
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
        _energy = YG2.saves.energy;
        _playerStats.currentEnergy.Value = _energy;
    }

    public void Die()
    {
        _aimer.SetCameraAim(_transform);

        Defeat();
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
    }
}
