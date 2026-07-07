using YG;

public class AddEnergyButton : EndScreenButton
{
    private string _rewardId = "AddPlayerEnergy";    

    public override void HandleClick()
    {
        _audioService.PlaySound(SoundType.ButtonClick);
        _audioService.Deactivate();

        YG2.RewardedAdvShow(_rewardId, () =>
        {
            YG2.saves.energy += 1;
            _audioService.Activate();
        });

        SetEnabled(false);
    }
}
