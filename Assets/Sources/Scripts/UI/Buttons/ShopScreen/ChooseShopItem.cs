using BlinkBlade.Game;
using BlinkBlade.Players;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace BlinkBlade.UI
{
    public class ChooseShopItem : UIButton
    {
        [SerializeField] private Toggle _toggle;
        [SerializeField] private ShopItem _item;

        [Inject] private PlayerStats _playerStats;

        public override void HandleClick()
        {
            _audioService.PlaySound(SoundType.ButtonClick);

            if (_toggle.IsActive() && _toggle.isOn == false)
            {
                _toggle.isOn = true;

                if (_item is WeaponItem)
                    _playerStats.CurrentWeaponId.Value = _item.Id;

                if (_item is SkinItem)
                    _playerStats.CurrentSkinId.Value = _item.Id;
            }
        }
    }
}