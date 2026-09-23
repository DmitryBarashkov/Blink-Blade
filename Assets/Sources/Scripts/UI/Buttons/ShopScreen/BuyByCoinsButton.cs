using System;

using BlinkBlade.Game;
using BlinkBlade.Players;

using UnityEngine;
using YG;
using Zenject;

namespace BlinkBlade.UI
{
    public class BuyByCoinsButton : UIButton
    {
        [Inject] private ShopService _shopService;
        [Inject] private PlayerStats _playerStats;

        [SerializeField] private ShopItem _shopItem;

        public override void HandleClick()
        {
            _audioService.PlaySound(SoundType.ButtonClick);

            YG2.saves.Coins -= _shopItem.Cost;

            if (YG2.saves.Coins < 0)
                throw new ArgumentOutOfRangeException(nameof(YG2.saves.Coins));

            YG2.SaveProgress();

            _playerStats.CurrentCoins.Value = YG2.saves.Coins;

            _shopService.PurchaseItem(_shopItem);
            _shopItem.UpdateAfterBuy();
        }
    }
}