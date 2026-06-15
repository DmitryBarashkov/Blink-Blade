using YG;
using Zenject;

public class NextLevelButton : EndScreenButton
{
    [Inject] private LevelLoadService _levelService;

    public override void HandleClick()
    {
        _audioService.PlaySound(SoundType.ButtonClick);

        if (YG2.saves.isAdsDisabled == false)
            YG2.InterstitialAdvShow();

        _screen.Close();
        _levelService.LoadLevel(YG2.saves.level).Forget();
    }
}
