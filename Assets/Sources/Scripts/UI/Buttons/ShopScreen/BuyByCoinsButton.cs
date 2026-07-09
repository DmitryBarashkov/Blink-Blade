using System;
using UnityEngine;
using YG;
using Zenject;

public class BuyByCoinsButton : UIButton
{
    [Inject] private ShopService _shopService;
    [Inject] private PlayerStats _playerStats;

    [SerializeField] private SkinItem _skinItem;

    public override void HandleClick()
    {
        _audioService.PlaySound(SoundType.ButtonClick);

        YG2.saves.coins -= _skinItem.Cost;

        if (YG2.saves.coins < 0)
            throw new ArgumentOutOfRangeException(nameof(YG2.saves.coins));

        YG2.SaveProgress();

        _playerStats.currentCoins.Value = YG2.saves.coins;
        
        _shopService.PurchaseSkin(_skinItem.SkinId);
        _skinItem.UpdateAfterBuy();
    }
}
