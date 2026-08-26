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

        YG2.saves.Coins -= _skinItem.Cost;

        if (YG2.saves.Coins < 0)
            throw new ArgumentOutOfRangeException(nameof(YG2.saves.Coins));

        YG2.SaveProgress();

        _playerStats.CurrentCoins.Value = YG2.saves.Coins;

        _shopService.PurchaseSkin(_skinItem.SkinId);
        _skinItem.UpdateAfterBuy();
    }
}
