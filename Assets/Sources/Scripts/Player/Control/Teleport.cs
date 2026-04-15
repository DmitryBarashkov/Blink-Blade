using UnityEngine;

public class Teleport
{
    private Weapon _weapon;
    private Transform _playerTransform;
    private CapsuleCollider _playerCollider;
    private Rigidbody _playerRigidbody;    

    private float _obstacleOffset = 1.5f;    

    public Teleport(Weapon weapon, Player player)
    {
        _weapon = weapon;
        _playerTransform = player.transform;
        _playerCollider = player.GetComponent<CapsuleCollider>();
        _playerRigidbody = player.GetComponent<Rigidbody>();        
    }

    public void Perform()
    {
        Vector3 newPosition = GetSafePosition(_weapon.transform.position);

        _playerRigidbody.velocity = Vector3.zero;
        _playerRigidbody.angularVelocity = Vector3.zero;
        _playerTransform.position = newPosition;
        
        _weapon.ReturnToWeaponHandler();
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
