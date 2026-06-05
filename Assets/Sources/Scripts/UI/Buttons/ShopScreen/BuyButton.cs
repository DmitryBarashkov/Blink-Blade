using UnityEngine;
using YG;
using Zenject;

public class BuyButton : UIButton
{
    [Inject] private ShopService _shopService;

    [SerializeField] private WeaponItem _weaponItem;

    private string _rewardId = "BuyNewWeapon";

    public override void HandleClick()
    {
        _audioService.PlaySound(SoundType.ButtonClick);

        YG2.RewardedAdvShow(_rewardId, () =>
        {
            _shopService.PurchaseItem(_weaponItem.WeaponId);
            _weaponItem.UpdateAfterBuy();
        });
    }
}
