using Zenject;

public class RestartButton : EndScreenButton
{
    [Inject] private Level _level;
    [Inject] private LevelState _levelState;

    public override void HandleClick()
    {
        _audioService.PlaySound(SoundType.ButtonClick);

        _level.Restart();
        _screen.Close();
        _levelState.EnergyUsed.Value = false;
    }
}
