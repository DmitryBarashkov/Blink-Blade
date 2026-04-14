using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Aimer
{
    private Camera _camera;
    private SmartFocusCamera _focusCamera;
    private RigBuilder _rigBuilder;
    private Transform _transform;
    private GameObject _aimingArrow;
    private Transform _weaponHandler;
    private Transform _targetTransform;
    private PlayerAnimator _animator;
    private Thrower _thrower;
    private Weapon _weapon;    

    private float _aimDistance = 10f;
    private float _maxTurnAngle = 100f;
            
    private Vector3 _targetDir;
    private float _currentAngle;    

    public Aimer(Transform playerTransform, SmartFocusCamera focusCamera, RigBuilder rigBuilder, 
                 Transform target, Transform weaponHandler, Weapon weapon, 
                 PlayerAnimator animator, GameObject aimingArrow)
    {
        _camera = focusCamera.GetComponent<Camera>();
        _focusCamera = focusCamera;
        _rigBuilder = rigBuilder;
        _targetTransform = target;        
        _transform = playerTransform;
        _aimingArrow = aimingArrow;
        
        _weaponHandler = weaponHandler;
        _weapon = weapon;

        _animator = animator;
        _thrower = new Thrower(_weapon.GetComponent<Rigidbody>());        
    }    
    
    public void StartAim()
    {
        _aimingArrow.SetActive(true);
        
        Plane plane = new Plane(Vector3.forward, _transform.position + _transform.forward * _aimDistance);
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        _rigBuilder.enabled = true;

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 mouseWorldPoint = ray.GetPoint(enter);

            mouseWorldPoint.z = _transform.position.z;

            _targetTransform.position = Vector3.Lerp(_targetTransform.position, mouseWorldPoint, Time.deltaTime * _aimDistance);
        }

        _animator.SetAiming(true);
    }

    public void StopAim()
    {
        Vector3 direction = (_targetTransform.position - _weaponHandler.position).normalized;

        _thrower?.Throw(direction);
        _focusCamera.SetTarget(_weapon.transform);

        _animator.SetAiming(false);

        _rigBuilder.enabled = false;
        _aimingArrow.SetActive(false);        
    }

    public void RotateToTarget()
    {
        _targetDir = Vector3.ProjectOnPlane(_targetTransform.position - _transform.position, Vector3.up);
        _currentAngle = Vector3.Angle(_transform.forward, _targetDir);

        if (_currentAngle > _maxTurnAngle)
        {
            _transform.rotation = Quaternion.LookRotation(_targetDir);
        }
    }
}
