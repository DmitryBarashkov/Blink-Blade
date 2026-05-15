using Zenject;

public class MusicButton : ToggleButton
{
    private SoundService _soundService;

    [Inject]
    public void Construct(SoundService soundService)
    {
        _soundService = soundService;

        _isOn = _soundService.GetMusicOn();
    }

    public override void HandleClick()
    {
        Toggle();

        _soundService.SetMusic(_isOn);

        SetSprite();
    }
}
