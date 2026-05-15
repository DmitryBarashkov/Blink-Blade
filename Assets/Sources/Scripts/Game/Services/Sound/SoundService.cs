using YG;
using Zenject;

public class SoundService
{
    private bool _isSoundOn;
    private bool _isMusicOn;

    [Inject]
    public void Construct()
    {
        _isMusicOn = YG2.saves.isMusicOn;
        _isSoundOn = YG2.saves.isSoundOn;
    }
    
    public bool GetSoundOn()
    {
        return _isSoundOn;
    }

    public bool GetMusicOn()
    {
        return _isMusicOn;
    }

    public void SetSound(bool value)
    {
        _isSoundOn = YG2.saves.isSoundOn = value;
    }

    public void SetMusic(bool value)
    {
        _isSoundOn = YG2.saves.isSoundOn = value;
    }
}
