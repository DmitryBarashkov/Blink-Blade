using BlinkBlade.Common;
using BlinkBlade.Game;
using UnityEngine;
using Zenject;

namespace BlinkBlade.UI
{
    public class BuyByAdsButton : UIButton
    {
        [Inject] private ShopService _shopService;
        [Inject] private AdService _adService;

        [SerializeField] private ShopItem _weaponItem;

        private string _rewardId = "BuyNewWeapon";

        public override void HandleClick()
        {
            _adService.ShowAdvForReward(_rewardId, GetAward);
        }

        private void GetAward()
        {
            _shopService.PurchaseItem(_weaponItem);
            _weaponItem.UpdateAfterBuy();
        }
    }
}