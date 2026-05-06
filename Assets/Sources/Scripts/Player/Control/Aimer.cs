using Cinemachine;
using UnityEngine;
using Zenject;

public class Aimer
{
    private CinemachineVirtualCamera _camera;
    private Transform _playerTransform;
    private AimingArrow _aimingArrow;
    private PlayerAnimator _animator;    
    private Weapon _weapon;    

    private float _maxTurnAngle = 100f;    

    private Vector3 _targetDir;
    private float _currentAngle;    

    [Inject]
    private void Construct(CinemachineVirtualCamera camera, Weapon weapon, AimingArrow aimingArrow)
    {
        _camera = camera;        
        _weapon = weapon;
        _aimingArrow = aimingArrow;
    }

    public void Initialize(Transform playerTransform, PlayerAnimator animator)
    {
        _animator = animator;

        _playerTransform = playerTransform;        

        _camera.Follow = _playerTransform;
        _camera.LookAt = _playerTransform;
    }

    public void StartAim()
    {
        SetCameraAim(_playerTransform);

        _animator.SetAiming(true);

        _aimingArrow.SetPosition(_playerTransform.position);
        _aimingArrow.Show();
    }

    public void StopAim()
    {
        _weapon.Throw(_aimingArrow.Direction, _playerTransform.rotation.y);

        _animator.SetAiming(false);

        _aimingArrow.Hide();
        SetCameraAim(_weapon.transform);
    }

    public void RotateToTarget()
    {
        _targetDir = Vector3.ProjectOnPlane(_aimingArrow.Direction, Vector3.up);
        _currentAngle = Vector3.Angle(_playerTransform.forward, _targetDir);

        if (_currentAngle > _maxTurnAngle)
        {
            _playerTransform.rotation = Quaternion.LookRotation(_targetDir);
        }
    }

    public void SetCameraAim(Transform transform)
    {
        _camera.Follow = transform;
        _camera.LookAt = transform;
    }
}
