using BlinkBlade.Game;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace BlinkBlade.Camera
{
    public class CameraBoundsInstaller
    {
        private readonly CinemachineVirtualCamera Camera;

        [Inject]
        public CameraBoundsInstaller(CinemachineVirtualCamera camera)
        {
            Camera = camera;
        }

        public void SetAim(Transform aim)
        {
            Camera.LookAt = aim;
            Camera.Follow = aim;
        }

        public void Initialize(ILevelData levelData)
        {
            PolygonCollider2D bounds = levelData.GetCameraBounds();
            CinemachineConfiner2D confiner = Camera.GetComponent<CinemachineConfiner2D>();

            if (confiner != null && bounds != null)
            {
                confiner.m_BoundingShape2D = bounds.GetComponent<Collider2D>();
                confiner.InvalidateCache();
            }
        }
    }
}