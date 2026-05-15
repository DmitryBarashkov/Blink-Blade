using Zenject;

public class SoundButton : ToggleButton
{
    private SoundService _soundService;

    [Inject]
    public void Construct(SoundService soundService)
    {
        _soundService = soundService;

        _isOn = _soundService.GetSoundOn();        
    }

    public override void HandleClick()
    {
        Toggle();

        _soundService.SetSound(_isOn);

        SetSprite();
    }
}
