using UnityEngine;
using UnityEngine.UI;

public abstract class ToggleButton : UIButton
{
    protected bool _isOn;

    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;
    [SerializeField] private Image _image;

    protected void Toggle()
    {
        _isOn = !_isOn;
    }

    protected void SetSprite()
    {
        _image.sprite = _isOn ? _onSprite : _offSprite;
    }
}
