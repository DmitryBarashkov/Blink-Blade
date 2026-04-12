using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Aimer : MonoBehaviour
{
    [SerializeField] private GameObject _aimingArrow;
    
    private float _aimDistance = 10f;

    private Camera _camera;
    private RigBuilder _rigBuilder;
    private Transform _transform;
    private Transform _weaponHandler;
    private Transform _targetTransform;
    private PlayerAnimator _animator;
    private Thrower _thrower;
    private Transform _weapon;
    private Rigidbody _weaponRb;

    private Coroutine _coroutine;

    private Vector3 _startWeaponPosition;
    private Quaternion _startWeaponRotation;

    private void Update()
    {
        Debug.DrawLine(_weaponHandler.position, _targetTransform.position);        
    }

    public void Initialize(Camera camera, RigBuilder rigBuilder, Transform target, Transform weaponHandler, Animator animator)
    {
        _camera = camera;
        _rigBuilder = rigBuilder;
        _targetTransform = target;        
        _transform = transform;
        
        _weaponHandler = weaponHandler;
        _weapon = weaponHandler.GetChild(0);
        _weaponRb = _weapon?.GetComponent<Rigidbody>();
        _startWeaponPosition = _weapon.localPosition;
        _startWeaponRotation = _weapon.localRotation;

        _animator = new PlayerAnimator();
        _animator.Initialize(animator);

        _thrower = new Thrower();
        _thrower.Initialize(_weaponRb, _weaponHandler);
    }    
    
    public void StartAim()
    {
        if (_coroutine != null)
            return;        
        
        _aimingArrow.SetActive(true);
        
        Plane plane = new Plane(Vector3.forward, _transform.position + _transform.forward * _aimDistance);
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        _rigBuilder.enabled = true;

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 mouseWorldPoint = ray.GetPoint(enter);

            _targetTransform.position = Vector3.Lerp(_targetTransform.position, mouseWorldPoint, Time.deltaTime * _aimDistance);
        }

        _animator.SetAiming(true);
    }

    public void StopAim()
    {
        if (_coroutine != null)
            return;

        Vector3 direction = (_targetTransform.position - _weaponHandler.position).normalized;

        _thrower?.Throw(direction);

        _animator.SetAiming(false);

        _rigBuilder.enabled = false;
        _aimingArrow.SetActive(false);

        _coroutine = StartCoroutine(ReturnWeapon());
    }

    private IEnumerator ReturnWeapon()
    {
        yield return new WaitForSeconds(3f);

        _weapon.localPosition = _startWeaponPosition;
        _weapon.localRotation = _startWeaponRotation;
        _weaponRb.velocity = Vector3.zero;
        _weaponRb.angularVelocity = Vector3.zero;
        _weaponRb.isKinematic = true;
        _coroutine = null;
    }
}
