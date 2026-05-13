using UnityEngine;
using Zenject;

public class ScreenResolutionAdapter : ITickable
{
    [Inject] private CanvasScaleAdapter _canvasAdapter;
    [Inject] private CameraResizer _cameraResizer;
    
    private int _lastWidth;
    private int _lastHeight;
    private ScreenOrientation _lastOrientation;

    [Inject]
    private void Construct()
    {
        ResetTrackedValues();
    }

    private void ResetTrackedValues()
    {
        _lastWidth = Screen.width;
        _lastHeight = Screen.height;
        _lastOrientation = Screen.orientation;
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
}
