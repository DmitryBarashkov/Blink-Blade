using BlinkBlade.Game;
using Zenject;

namespace BlinkBlade.UI
{
    public class WeaponItem : ShopItem
    {
        [Inject]
        public override void Construct(ShopService shopService)
        {
            _shopService = shopService;

            Id = _equip.Id;
            Cost = _equip.Cost;
            IsChosen = _shopService.IsWeaponChosen(Id);
            IsPurchased = _shopService.IsWeaponItemPurchased(Id);

            if (_buyButtonText != null)
                _buyButtonText.text = _equip.Cost.ToString();

            InitializeItem();
            InitializeToggleControl();
        }

        public override void UpdateItem()
        {
            IsChosen = _shopService.IsWeaponChosen(Id);
            IsPurchased = _shopService.IsWeaponItemPurchased(Id);

            InitializeItem();
            InitializeToggleControl();
        }
    }
}
