using UnityEngine;
using Cinemachine;
using Zenject;

public class CameraResizer
{
    private CinemachineVirtualCamera _camera;
    private CinemachineTransposer _transposer;

    private float _portraitOrthoSize = 10f;
    private float _albumOrthoSize = 4f;
    private Vector3 _portraitFollowOffset = new Vector3(0, 6f, -10f);
    private Vector3 _albumFollowOffset = new Vector3(0, 1f, -10f);

    [Inject]
    private void Construct(CinemachineVirtualCamera camera)
    {
        _camera = camera;
        _transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
        AdjustCameraSize();
    }
    
    public void AdjustCameraSize()
    {
        if (_camera == null)
            return;

        bool isPortrait = Screen.width < Screen.height;

        _camera.m_Lens.OrthographicSize = isPortrait ? _portraitOrthoSize : _albumOrthoSize;
        _transposer.m_FollowOffset = isPortrait ? _portraitFollowOffset : _albumFollowOffset;
    }
}
