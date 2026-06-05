using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            YG2.saves.energy += 1;
            _playerStats.currentEnergy.Value += 1;
        });
    }
}
