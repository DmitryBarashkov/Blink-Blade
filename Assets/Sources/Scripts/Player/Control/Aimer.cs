using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Zenject;

public class Aimer
{
    private CinemachineVirtualCamera _camera;
    private RigBuilder _rigBuilder;
    private Transform _playerTransform;
    private AimingArrow _aimingArrow;
    private WeaponHandler _weaponHandler;
    private Target _target;
    private PlayerAnimator _animator;
    private Thrower _thrower;
    private Weapon _weapon;    

    private float _aimDistance = 10f;
    private float _maxTurnAngle = 100f;
    private float _targetWeight = 1f;
            
    private Vector3 _targetDir;
    private float _currentAngle;    

    [Inject]
    private void Construct(CinemachineVirtualCamera camera, Target target, Weapon weapon)
    {
        _camera = camera;
        _target = target;
        _weapon = weapon;

        _thrower = new Thrower(_weapon);
    }

    public void Initialize(Transform playerTransform, RigBuilder rigBuilder, PlayerAnimator animator, AimingArrow aimingArrow, WeaponHandler weaponHandler)
    {
        _rigBuilder = rigBuilder;
        SetRigBuilderTarget();
        
        _animator = animator;

        _playerTransform = playerTransform;
        _aimingArrow = aimingArrow;
        _weaponHandler = weaponHandler;

        _camera.Follow = _playerTransform;
        _camera.LookAt = _playerTransform;
    }

    public void StartAim()
    {
        _camera.Follow = _playerTransform;
        _camera.LookAt = _playerTransform;
        _aimingArrow.Show();

        Plane plane = new Plane(Vector3.forward, _playerTransform.position + _playerTransform.forward * _aimDistance);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        _rigBuilder.enabled = true;

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 mouseWorldPoint = ray.GetPoint(enter);

            mouseWorldPoint.z = _playerTransform.position.z;

            _target.SetPosition(mouseWorldPoint);            
        }

        _animator.SetAiming(true);
    }

    public void StopAim()
    {
        Vector3 direction = (_target.transform.position - _weaponHandler.transform.position).normalized;

        _weapon.ResetRotation(_playerTransform.rotation.y);        
        _thrower.Throw(direction);

        _animator.SetAiming(false);

        _rigBuilder.enabled = false;
        _aimingArrow.Hide();
        _camera.Follow = _weapon.transform;
        _camera.LookAt = _weapon.transform;
    }

    public void RotateToTarget()
    {
        _targetDir = Vector3.ProjectOnPlane(_target.transform.position - _playerTransform.position, Vector3.up);
        _currentAngle = Vector3.Angle(_playerTransform.forward, _targetDir);

        if (_currentAngle > _maxTurnAngle)
        {
            _playerTransform.rotation = Quaternion.LookRotation(_targetDir);
        }
    }

    private void SetRigBuilderTarget()
    {
        MultiAimConstraint aimConstraint = _rigBuilder.GetComponentInChildren<MultiAimConstraint>();
        var weightedTransform = new WeightedTransform(_target.transform, _targetWeight);
        var sourceArray = new WeightedTransformArray { weightedTransform };
        var animator = _rigBuilder.GetComponent<Animator>();

        if (aimConstraint == null)
            throw new ArgumentNullException(nameof(aimConstraint));

        if (animator == null)
            throw new ArgumentNullException(nameof(animator));

        aimConstraint.data.sourceObjects = sourceArray;

        if (animator != null)
            animator.enabled = true;

        _rigBuilder.Build();
    }
}
