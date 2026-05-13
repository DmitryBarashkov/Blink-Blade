using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CanvasScaleAdapter
{
    private CanvasScaler[] _canvasScalers;
    private Vector2 _portraitResolution = new Vector2(1080, 1920);
    private Vector2 _albumResolution = new Vector2(1920, 1080);

    [Inject]
    private void Construct(CanvasScaler[] canvasScalers)
    {
        _canvasScalers = canvasScalers;
        ApplyScaleMode();
    }

    public void ApplyScaleMode()
    {
        bool isPortrait = Screen.height > Screen.width;

        foreach (CanvasScaler scaler in _canvasScalers)
        {
            scaler.referenceResolution = isPortrait ? _portraitResolution : _albumResolution;
        }
    }
}
