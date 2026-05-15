using YG;
using Zenject;

public class RestartButton : EndScreenButton
{
    [Inject] private Level _level;
    [Inject] private LevelState _levelState;

    public override void HandleClick()
    {
        if (YG2.saves.IsAdsDisabled == false)
            YG2.InterstitialAdvShow();

        _level.Restart();
        _screen.Close();
        _levelState.EnergyUsed.Value = false;
    }
}
