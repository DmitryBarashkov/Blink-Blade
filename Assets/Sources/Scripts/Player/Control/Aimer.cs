using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Aimer
{
    private CinemachineVirtualCamera _camera;
    private RigBuilder _rigBuilder;
    private Transform _playerTransform;
    private AimingArrow _aimingArrow;
    private Transform _weaponHandler;
    private Target _target;
    private PlayerAnimator _animator;
    private Thrower _thrower;
    private Weapon _weapon;    

    private float _aimDistance = 10f;
    private float _maxTurnAngle = 100f;
            
    private Vector3 _targetDir;
    private float _currentAngle;    

    public Aimer(CinemachineVirtualCamera camera, Transform playerTransform, RigBuilder rigBuilder,
                 Target target, Transform weaponHandler, Weapon weapon, 
                 PlayerAnimator animator, AimingArrow aimingArrow)
    {
        _camera = camera;
        
        _rigBuilder = rigBuilder;
        _target = target;        
        _playerTransform = playerTransform;
        _aimingArrow = aimingArrow;
        
        _weaponHandler = weaponHandler;
        _weapon = weapon;

        _animator = animator;
        _thrower = new Thrower(_weapon);
    }    
    
    public void StartAim()
    {
        _camera.Follow = _playerTransform;
        _aimingArrow.Show();

        Plane plane = new Plane(Vector3.forward, _playerTransform.position + _playerTransform.forward * _aimDistance);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        _rigBuilder.enabled = true;

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 mouseWorldPoint = ray.GetPoint(enter);

            mouseWorldPoint.z = _playerTransform.position.z;

            _target.SetPosition(mouseWorldPoint);
            //_aimingArrow.SetPosition(mouseWorldPoint);
        }

        _animator.SetAiming(true);
    }

    public void StopAim()
    {
        Vector3 direction = (_target.transform.position - _weaponHandler.position).normalized;

        _weapon.ResetRotation(_playerTransform.rotation.y);        
        _thrower?.Throw(direction);

        _animator.SetAiming(false);

        _rigBuilder.enabled = false;
        _aimingArrow.Hide();
        _camera.Follow = _weapon.transform;        
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
}
