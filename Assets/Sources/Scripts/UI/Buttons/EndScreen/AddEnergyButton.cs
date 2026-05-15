using YG;

public class AddEnergyButton : EndScreenButton
{
    private string _rewardId = "AddPlayerEnergy";    

    public override void HandleClick()
    {
        YG2.RewardedAdvShow(_rewardId, () =>
        {
            YG2.saves.energy += 1;
        });

        Disable();
    }
}
