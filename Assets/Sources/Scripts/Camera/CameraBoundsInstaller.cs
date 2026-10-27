using BlinkBlade.Game;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace BlinkBlade.Camera
{
    public class CameraBoundsInstaller
    {
        private readonly CinemachineVirtualCamera Camera;

        private float _xDamping = 0.25f;
        private float _yDamping = 0.25f;

        private bool _isClearedDamping;

        [Inject]
        public CameraBoundsInstaller(CinemachineVirtualCamera camera)
        {
            Camera = camera;
        }

        public void SetAim(Transform aim, bool isInitSet = false)
        {
            if (isInitSet)
            {
                ClearDamping();
                Camera.LookAt = aim;
                Camera.Follow = aim;
            }
            else
            {
                if (_isClearedDamping)
                {
                    var transposer = Camera.GetCinemachineComponent<CinemachineTransposer>();

                    if (transposer != null)
                    {
                        transposer.m_XDamping = _xDamping;
                        transposer.m_YDamping = _yDamping;
                    }

                    _isClearedDamping = false;
                }

                Camera.LookAt = aim;
                Camera.Follow = aim;
            }
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

        public void ClearDamping()
        {
            var transposer = Camera.GetCinemachineComponent<CinemachineTransposer>();

            if (transposer != null)
            {
                transposer.m_XDamping = 0;
                transposer.m_YDamping = 0;

                _isClearedDamping = true;
            }
        }
    }
}