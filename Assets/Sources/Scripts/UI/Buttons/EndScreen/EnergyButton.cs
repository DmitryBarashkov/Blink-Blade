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
        Utils.ShowAdvForReward(_audioService, _rewardId, GetAward);
    }

    private void GetAward()
    {
        _playerStats.currentEnergy.Value += _addCount;

        _screen.Close();
        _levelState.IsWin.Value = null;
        _levelState.EnergyUsed.Value = true;
        _level.StartPlay(true);
    }
}
