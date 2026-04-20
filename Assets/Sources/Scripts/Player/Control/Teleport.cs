using UnityEngine;

public class Teleport
{
    private Weapon _weapon;
    private Transform _playerTransform;
    private CapsuleCollider _safePositionCollider;    
    private Rigidbody _playerRigidbody;

    private float _playerHeight;
    private LayerMask _obstacleMask = LayerMask.GetMask("Ground");
    private float _horizontalOffset = 1.6f;
    private float _verticalOffset = 1.6f;

    public Teleport(Weapon weapon, Player player)
    {
        _weapon = weapon;
        _playerTransform = player.transform;
        _safePositionCollider = player.GetComponent<CapsuleCollider>();
        _playerRigidbody = player.GetComponent<Rigidbody>();

        _playerHeight = _safePositionCollider.height * _playerTransform.lossyScale.y;
    }

    public void Perform()
    {
        Vector3 newPosition = GetSafePosition();

        _playerRigidbody.velocity = Vector3.zero;
        _playerRigidbody.angularVelocity = Vector3.zero;
        _playerTransform.position = newPosition;
        
        _weapon.ReturnToWeaponHandler();
    }

    private Vector3 GetSafePosition()
    {
        Vector3 finalPosition = _weapon.transform.position;

        finalPosition = GetCorrectedHorizontalPosition(finalPosition, Vector3.right);
        finalPosition = GetCorrectedHorizontalPosition(finalPosition, Vector3.left);
        finalPosition = GetCorrectedCeilingPosition(finalPosition);
        finalPosition = GetCorrectedFloorPosition(finalPosition);

        return finalPosition;
    }

    private Vector3 GetCorrectedHorizontalPosition(Vector3 targetPosition, Vector3 direction)
    {
        if (Physics.Raycast(targetPosition, direction, out RaycastHit hit, _horizontalOffset, _obstacleMask))
        {
            targetPosition.x = hit.point.x - (direction.x * _horizontalOffset);
        }

        return targetPosition;
    }

    private Vector3 GetCorrectedCeilingPosition(Vector3 position)
    {
        float halfHeight = _playerHeight / 2f;
        float checkDistance = halfHeight + _verticalOffset;

        if (Physics.Raycast(position, Vector3.up, out RaycastHit hit, checkDistance, _obstacleMask))
        {        
            float overlap = checkDistance - hit.distance;

            if (overlap > 0)
                position.y -= overlap;
        }

        return position;
    }

    private Vector3 GetCorrectedFloorPosition(Vector3 position)
    {
        float halfHeight = _playerHeight / 2f;
        float checkDist = halfHeight + _horizontalOffset;        
        
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, checkDist, _obstacleMask))
        {
            position.y = hit.point.y;
        }

        return position;
    }
}
