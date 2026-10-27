using BlinkBlade.Players;
using UnityEngine;
using YG;
using Zenject;

namespace BlinkBlade.UI
{
    public class DebugCoinsButton : UIButton
    {
        [SerializeField] private RectTransform _screen;

        [Inject] private PlayerStats _playerStats;

        private int _coinsBoost = 5000;

        public override void HandleClick()
        {
            YG2.saves.Coins += _coinsBoost;
            _playerStats.CurrentCoins.Value = YG2.saves.Coins;

            _screen.gameObject.SetActive(false);
        }
    }
}
