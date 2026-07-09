using YG;
using Zenject;

public class EnergyButton : EndScreenButton
{
    [Inject] private Level _level;
    [Inject] private LevelState _levelState;
    [Inject] private PlayerStats _playerStats;
    

    private int _addCount = 3;
    private string _rewardId = "AddEnergyForLevel";

    public override void HandleClick()
    {
        _audioService.PlaySound(SoundType.ButtonClick);

        YG2.RewardedAdvShow(_rewardId, () =>
        {
            _playerStats.currentEnergy.Value += _addCount;

            _screen.Close();
            _levelState.IsWin.Value = null;
            _levelState.EnergyUsed.Value = true;
            _level.StartPlay(true);
        });        
    }
}
