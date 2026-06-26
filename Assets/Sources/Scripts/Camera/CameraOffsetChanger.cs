using Cinemachine;
using System;
using UnityEngine;
using Zenject;

public class CameraOffsetChanger
{
    private CinemachineTransposer _transposer;
    
    private float _smoothTime = 0.75f;
    private float _offsetHorizontal = 4f;
    private float _offsetUp = 3f;
    private float _offsetDown = 1f;
    private float _initialOffsetZ = -10;

    private Vector3 _targetOffset;
    private Vector3 _initialOffset;

    private Vector3 _offsetVelocity;

    [Inject]
    public void Construct(CinemachineVirtualCamera camera)
    {
        _transposer = camera.GetCinemachineComponent<CinemachineTransposer>();
        _initialOffset = _transposer.m_FollowOffset;
    }

    public void SetOffset(Vector3 direction)
    {
        if (_transposer == null)
            throw new ArgumentNullException(nameof(_transposer));

        if (direction == null)
            throw new ArgumentNullException(nameof(direction));

        float positionX = direction.x * _offsetHorizontal;
        float PositionY = direction.y > 0 ? direction.y * _offsetUp : direction.y * _offsetDown;

        _targetOffset = new Vector3(positionX, PositionY, _initialOffsetZ);
        _transposer.m_FollowOffset = Vector3.SmoothDamp(_transposer.m_FollowOffset, _targetOffset, ref _offsetVelocity, _smoothTime);
    }

    public void ClearOffset(bool isInstantClear)
    {
        if (isInstantClear)
            _transposer.m_FollowOffset = _initialOffset;
        else
            _transposer.m_FollowOffset = Vector3.SmoothDamp(_transposer.m_FollowOffset, _initialOffset, ref _offsetVelocity, _smoothTime);
    }
}
