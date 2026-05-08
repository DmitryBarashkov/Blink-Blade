using UnityEngine;
using YG;
using Zenject;

public class EnergyButton : UIButton
{
    [SerializeField] private EndGameWindow _window;

    [Inject] private Level _level;
    [Inject] private LevelState _levelState;
    [Inject] private Player _player;

    private int _addCount = 3;
    private string _rewardId = "AddEnergyForLevel";

    public override void HandleClick()
    {
        YG2.RewardedAdvShow(_rewardId, () =>
        {
            _player.AddEnergy(_addCount);

            _window.Close();
            _levelState.IsWin.Value = null;
            _levelState.EnergyUsed.Value = true;
            _level.StartPlay();
        });

        gameObject.SetActive(false);
    }
}
