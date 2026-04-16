using UnityEngine;

public class Teleport
{
    private Weapon _weapon;
    private Transform _playerTransform;
    private CapsuleCollider _safePositionCollider;    
    private Rigidbody _playerRigidbody;    

    public Teleport(Weapon weapon, Player player, CapsuleCollider checkerCollider)
    {
        _weapon = weapon;
        _playerTransform = player.transform;
        _safePositionCollider = checkerCollider;
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

    private Vector3 GetSafePosition(Vector3 targetPosition)
    {
        LayerMask obstacleMask = LayerMask.GetMask("Ground");
        Vector3 origin = _playerTransform.position;
        Vector3 direction = targetPosition - origin;

        float playerRadius = _safePositionCollider.radius * _playerTransform.lossyScale.x;
        float distance = direction.magnitude + playerRadius;
        float playerHeight = _safePositionCollider.height * _playerTransform.lossyScale.y;

        if (Physics.SphereCast(origin, playerRadius, direction.normalized, out RaycastHit hit, distance, obstacleMask))
        {
            Vector3 safePosition = hit.point + (hit.normal * playerRadius);
                    
            if (Vector3.Dot(hit.normal, Vector3.down) > 0.7f)
                safePosition.y = hit.point.y - playerHeight;

            return safePosition;
        }
        
        return targetPosition;
    }
}
