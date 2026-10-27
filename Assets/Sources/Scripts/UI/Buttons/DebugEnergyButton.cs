using BlinkBlade.Players;
using UnityEngine;
using YG;
using Zenject;

namespace BlinkBlade.UI
{
    public class DebugEnergyButton : UIButton
    {
        [SerializeField] private RectTransform _screen;

        [Inject] private PlayerStats _playerStats;

        private int _energyBoost = 50;

        public override void HandleClick()
        {
            YG2.saves.Energy += _energyBoost;
            _playerStats.CurrentEnergy.Value = YG2.saves.Energy;

            _screen.gameObject.SetActive(false);
        }
    }
}