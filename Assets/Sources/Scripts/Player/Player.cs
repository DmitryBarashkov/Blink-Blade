using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] Transform _targetTransform;
    [SerializeField] Transform _weaponHandler;
    
    private Animator _animator;
    private RigBuilder _rigBuilder;
    private InputService _input;
    private Aimer _aimer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigBuilder = GetComponent<RigBuilder>();
        _input = GetComponent<InputService>();
        _aimer = GetComponent<Aimer>();

        _aimer.Initialize(_camera, _rigBuilder, _targetTransform, _weaponHandler, _animator);
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

    private void OnAttackButtonUp()
    {
        _aimer.StopAim();
    }

    private void OnAttackButtonPressed()
    {
        _aimer.StartAim();
    }

    private void Update()
    {
        _input.GetInput();
    }
}
