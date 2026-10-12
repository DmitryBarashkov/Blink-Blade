using BlinkBlade.Common;
using BlinkBlade.Game;
using BlinkBlade.Players;
using Zenject;

namespace BlinkBlade.UI
{
    public class EnergyButton : EndScreenButton
    {
        [Inject] private Level _level;
        [Inject] private LevelState _levelState;
        [Inject] private PlayerStats _playerStats;
        [Inject] private AdService _adService;

        private int _addCount = 3;
        private string _rewardId = "AddEnergyForLevel";

        public override void HandleClick()
        {
            _adService.ShowAdvForReward(_rewardId, GetAward);
        }

        private void GetAward()
        {
            _playerStats.CurrentEnergy.Value += _addCount;

            _screen.Close();
            _levelState.IsWin.Value = null;
            _levelState.EnergyUsed.Value = true;
            _level.StartPlay(true);
        }
    }
}