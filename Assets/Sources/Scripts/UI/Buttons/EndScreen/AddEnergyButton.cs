using BlinkBlade.Common;
using BlinkBlade.Game;
using BlinkBlade.Players;
using YG;
using Zenject;

namespace BlinkBlade.UI
{
    public class AddEnergyButton : EndScreenButton
    {
        [Inject] private PlayerStats _playerStats;
        [Inject] private AdService _adService;

        private string _rewardId = "AddPlayerEnergy";

        public override void HandleClick()
        {
            _adService.ShowAdvForReward(_rewardId, GetAward);
        }

        private void GetAward()
        {
            YG2.saves.Energy += 1;
            _playerStats.CurrentEnergy.Value += YG2.saves.Energy;
            SetEnabled(false);
        }
    }
}