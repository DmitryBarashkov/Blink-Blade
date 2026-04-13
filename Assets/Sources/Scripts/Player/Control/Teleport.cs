using UnityEngine;

public class Teleport
{
    private Weapon _weapon;
    private Transform _playerTransform;
    private CapsuleCollider _playerCollider;
    private Rigidbody _weaponRigidbody;
    private Transform _weaponHandler;
    private SmartFocusCamera _focusCamera;

    private Vector3 _startWeaponPosition;
    private Quaternion _startWeaponRotation;

    private float _obstacleOffset = 1f;    

    public Teleport(Weapon weapon, Transform playerTransform, CapsuleCollider playerCollider, SmartFocusCamera focusCamera, Transform weaponHandler)
    {
        _weapon = weapon;
        _weaponRigidbody = weapon.GetRigidbody();
        _playerTransform = playerTransform;
        _playerCollider = playerCollider;
        _focusCamera = focusCamera;
        _weaponHandler = weaponHandler;

        _startWeaponPosition = _weapon.transform.localPosition;
        _startWeaponRotation = _weapon.transform.localRotation;
    }

    public void Perform()
    {
        Vector3 newPosition = GetSafePosition(_weapon.transform.position);

        _playerTransform.position = newPosition;
        ReturnWeapon();
    }

    private void ReturnWeapon()
    {
        _weapon.transform.SetParent(_weaponHandler);
        _weapon.transform.localPosition = _startWeaponPosition;
        _weapon.transform.localRotation = _startWeaponRotation;

        _weaponRigidbody.velocity = Vector3.zero;
        _weaponRigidbody.angularVelocity = Vector3.zero;
        _weaponRigidbody.isKinematic = true;

        _focusCamera.ResetToPlayer();
    }

    private Vector3 GetSafePosition(Vector3 targetPos)
    {
        float playerRadius = _playerCollider.radius;
        float playerHeight = _playerCollider.height;
        float scaledHeight = playerHeight * _playerTransform.lossyScale.y;
        float scaledRadius = playerRadius * Mathf.Max(_playerTransform.lossyScale.x, _playerTransform.lossyScale.z);
        LayerMask obstacleMask = LayerMask.GetMask("Ground");

        Vector3 point1 = targetPos + Vector3.up * scaledRadius;
        Vector3 point2 = targetPos + Vector3.up * (scaledHeight - playerRadius);

        if (Physics.CheckCapsule(point1, point2, scaledRadius, obstacleMask))
        {
            Vector3 directionToPlayer = (_playerTransform.position - targetPos).normalized;

            return targetPos + directionToPlayer * _obstacleOffset;
        }

        targetPos.z = _playerTransform.position.z;

        return targetPos;
    }
}
