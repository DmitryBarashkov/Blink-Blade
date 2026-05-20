using Zenject;

public class SoundButton : ToggleButton
{
    [Inject]
    public void Construct(AudioService soundService)
    {
        _isOn = _audioService.GetSoundOn();        
    }

    public override void HandleClick()
    {
        if (_isOn)
            _audioService.PlaySound(SoundType.ButtonClick);


        Toggle();

        _audioService.SetSound(_isOn);

        SetSprite();
    }
}
