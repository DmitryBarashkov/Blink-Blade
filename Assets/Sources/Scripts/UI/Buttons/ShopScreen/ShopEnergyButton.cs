using YG;
using Zenject;

public class ShopEnergyButton : UIButton
{
    [Inject] private PlayerStats _playerStats;
    
    private string _rewardId = "AddPlayerEnergy";

    public override void HandleClick()
    {
        _audioService.PlaySound(SoundType.ButtonClick);

        YG2.RewardedAdvShow(_rewardId, () =>
        {
            YG2.saves.energy++;
            _playerStats.currentEnergy.Value++;
        });
    }
}
