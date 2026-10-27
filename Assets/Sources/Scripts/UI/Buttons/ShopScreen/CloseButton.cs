using BlinkBlade.Game;
using UnityEngine;

namespace BlinkBlade.UI
{
    public class CloseButton : UIButton
    {
        [SerializeField] private ShopScreen _shopGameScreen;

        public override void HandleClick()
        {
            _audioService.PlaySound(SoundType.ButtonClick);
            _shopGameScreen.Close();
        }
    }
}