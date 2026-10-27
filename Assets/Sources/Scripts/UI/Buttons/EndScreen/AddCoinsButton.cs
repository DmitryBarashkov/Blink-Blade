using BlinkBlade.Game;
using UnityEngine;
using Zenject;

namespace BlinkBlade.UI
{
    public class AddCoinsButton : EndScreenButton
    {
        [Inject] private AdService _adService;

        [SerializeField] private WinGameScreen _winScreen;

        private string _rewardId = "MultiplyCoins";
        private int _coinsFactor = 2;

        public override void HandleClick()
        {
            _adService.ShowAdvForReward(_rewardId, GetAward);
        }

        private void GetAward()
        {
            _winScreen.AddCoins(_coinsFactor);
            SetEnabled(false);
        }
    }
}