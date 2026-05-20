using UnityEngine;
using Zenject;

public class Teleport
{
    private Weapon _weapon;
    private Transform _playerTransform;
    private CapsuleCollider _safePositionCollider;    
    private Rigidbody _playerRigidbody;
    private EffectsSpawner _effectsSpawner;
    private IAudioService _audioService;

    private float _playerHeight;
    private LayerMask _obstacleMask = LayerMask.GetMask("Ground");
    private float _horizontalOffset = 0.8f;
    private float _verticalOffset = 1.8f;

    [Inject]
    private void Construct(EffectsSpawner effectsSpawner, IAudioService audioService)
    {
        _effectsSpawner = effectsSpawner;
        _audioService = audioService;
    }

    public void Initialize(Weapon weapon, Transform playerTransform, CapsuleCollider collider, Rigidbody rigidbody)
    {
        _weapon = weapon;
        _playerTransform = playerTransform;
        _safePositionCollider = collider;
        _playerRigidbody = rigidbody;

        _playerHeight = _safePositionCollider.height * _playerTransform.lossyScale.y;
    }

    public void Perform()
    {
        Vector3 newPosition = GetSafePosition();
        Vector3 centerPlayerPosition = Vector3.up * _playerHeight / 2;
        Vector3 startLinePosition = _playerTransform.position + centerPlayerPosition;
        Vector3 endLinePosition = newPosition;

        _playerRigidbody.velocity = Vector3.zero;
        _playerRigidbody.angularVelocity = Vector3.zero;
        _playerTransform.position = newPosition;

        _weapon.ReturnToWeaponHandler();

        _effectsSpawner.SpawnTrailEffect(startLinePosition, endLinePosition);
        _effectsSpawner.SpawnTeleportEffect(_playerTransform);
        _audioService.PlaySound(SoundType.Teleport);
    }

    private Vector3 GetSafePosition()
    {
        Vector3 finalPosition = _weapon.transform.position;
        float halfHeight = _playerHeight / 2f;

        finalPosition = GetCorrectedHorizontalPosition(finalPosition, Vector3.right);
        finalPosition = GetCorrectedHorizontalPosition(finalPosition, Vector3.left);
        finalPosition = GetCorrectedCeilingPosition(halfHeight, finalPosition);
        finalPosition = GetCorrectedFloorPosition(halfHeight, finalPosition);

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

    private Vector3 GetCorrectedCeilingPosition(float halfHeight, Vector3 position)
    {
        float checkDistance = halfHeight + _verticalOffset;

        if (Physics.Raycast(position, Vector3.up, out RaycastHit hit, checkDistance, _obstacleMask))
        {        
            float overlap = checkDistance - hit.distance;

            if (overlap > 0)
                position.y -= overlap;
        }

        return position;
    }

    private Vector3 GetCorrectedFloorPosition(float halfHeight, Vector3 position)
    {
        float checkDist = halfHeight + _horizontalOffset;        
        
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, checkDist, _obstacleMask))
        {
            position.y = hit.point.y;
        }

        return position;
    }
}
