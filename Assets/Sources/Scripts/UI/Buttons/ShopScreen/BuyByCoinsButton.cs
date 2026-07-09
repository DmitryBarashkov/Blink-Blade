using UnityEngine;
using YG;
using Zenject;

public class BuyByCoinsButton : UIButton
{
    [Inject] private ShopService _shopService;

    [SerializeField] private SkinItem _skinItem;

    public override void HandleClick()
    {
        _audioService.PlaySound(SoundType.ButtonClick);

        YG2.saves.coins -= _skinItem.Cost;
        YG2.SaveProgress();
        
        _shopService.PurchaseSkin(_skinItem.SkinId);
        _skinItem.UpdateAfterBuy();
    }
}
