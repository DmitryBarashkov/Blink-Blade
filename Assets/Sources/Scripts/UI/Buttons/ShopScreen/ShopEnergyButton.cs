using BlinkBlade.Common;
using BlinkBlade.Players;
using YG;
using Zenject;

namespace BlinkBlade.UI
{
    public class ShopEnergyButton : UIButton
    {
        [Inject] private PlayerStats _playerStats;

        private string _rewardId = "AddPlayerEnergy";

        public override void HandleClick()
        {
            CommonFunctions.ShowAdvForReward(_audioService, _rewardId, GetAward);
        }

        private void GetAward()
        {
            YG2.saves.Energy++;
            _playerStats.CurrentEnergy.Value++;
        }
    }
}