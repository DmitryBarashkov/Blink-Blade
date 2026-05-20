using Zenject;

public class MusicButton : ToggleButton
{
    private AudioService _soundService;

    [Inject]
    public void Construct(AudioService soundService)
    {
        _soundService = soundService;

        _isOn = _soundService.GetMusicOn();
    }

    public override void HandleClick()
    {
        _audioService.PlaySound(SoundType.ButtonClick);

        Toggle();

        _soundService.SetMusic(_isOn);

        SetSprite();
    }
}
