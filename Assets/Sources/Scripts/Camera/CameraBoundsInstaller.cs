using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraBoundsInstaller
{
    private readonly CinemachineVirtualCamera _camera;

    [Inject]
    public CameraBoundsInstaller(CinemachineVirtualCamera camera)
    {
        _camera = camera;
    }

    public void SetAim(Transform aim)
    {
        _camera.LookAt = aim;
        _camera.Follow = aim;
    }

    public void Initialize(ILevelData levelData)
    {
        CameraBounds bounds = levelData.GetCameraBounds();
        CinemachineConfiner2D confiner = _camera.GetComponent<CinemachineConfiner2D>();

        if (confiner != null && bounds != null)
        {
            confiner.m_BoundingShape2D = bounds.GetComponent<Collider2D>();
            confiner.InvalidateCache();
        }
    }
}
