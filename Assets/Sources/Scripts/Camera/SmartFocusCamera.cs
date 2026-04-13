using UnityEngine;

public class SmartFocusCamera : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    private Vector3 _offset = new Vector3(0, 2, -10);
    private float _smoothSpeed = 5f;

    private Transform _currentTarget;
    private Transform _transform;
    private Vector3 _desiredPosition;

    private void Awake()
    {
        _transform = transform;
        _currentTarget = _playerTransform;
    }

    private void LateUpdate()
    {
        if (_currentTarget == null) return;

        _desiredPosition = _currentTarget.position + _offset;

        
        transform.position = Vector3.Lerp(_transform.position, _desiredPosition, _smoothSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform target)
    {
        _currentTarget = target; 
    }

    public void ResetToPlayer()
    {
        _currentTarget = _playerTransform;
    }
}
