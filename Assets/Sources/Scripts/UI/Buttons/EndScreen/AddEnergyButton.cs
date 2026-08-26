using YG;
using Zenject;

public class AddEnergyButton : EndScreenButton
{
    [Inject] private PlayerStats _playerStats;

    private string _rewardId = "AddPlayerEnergy";

    public override void HandleClick()
    {
        Utils.ShowAdvForReward(_audioService, _rewardId, GetAward);
    }

    private void GetAward()
    {
        YG2.saves.Energy += 1;
        _playerStats.CurrentEnergy.Value += YG2.saves.Energy;
        SetEnabled(false);
    }
}
