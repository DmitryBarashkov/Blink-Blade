using UnityEngine;
using Zenject;

public class BuyByAdsButton : UIButton
{
    [Inject] private ShopService _shopService;

    [SerializeField] private WeaponItem _weaponItem;

    private string _rewardId = "BuyNewWeapon";

    public override void HandleClick()
    {
        Utils.ShowAdvForReward(_audioService, _rewardId, GetAward);
    }

    private void GetAward()
    {
        _shopService.PurchaseWeapon(_weaponItem.WeaponId);
        _weaponItem.UpdateAfterBuy();
    }
}
