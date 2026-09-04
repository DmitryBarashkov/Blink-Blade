using UnityEngine;
using Zenject;

public class ScreenResolutionAdapter : ITickable
{
    private CanvasScaleAdapter _canvasAdapter;
    private CameraResizer _cameraResizer;

    private int _lastWidth;
    private int _lastHeight;
    private ScreenOrientation _lastOrientation;

    [Inject]
    public void Construct(CanvasScaleAdapter canvasScaleAdapter, CameraResizer cameraResizer)
    {
        _cameraResizer = cameraResizer;
        _canvasAdapter = canvasScaleAdapter;

        ResetTrackedValues();
    }

    public void Tick()
    {
        if (Screen.width != _lastWidth || Screen.height != _lastHeight || Screen.orientation != _lastOrientation)
        {
            ResetTrackedValues();

            _cameraResizer.AdjustCameraSize();
            _canvasAdapter.ApplyScaleMode();
        }
    }

    private void ResetTrackedValues()
    {
        _lastWidth = Screen.width;
        _lastHeight = Screen.height;
        _lastOrientation = Screen.orientation;
    }
}
