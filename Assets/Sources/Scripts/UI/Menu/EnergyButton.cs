using UnityEngine;
using Zenject;

public class EnergyButton : UIButton
{
    [SerializeField] private EndGameWindow _window;

    [Inject] private Level _level;
    [Inject] private LevelState _levelState;
    [Inject] private Player _player;

    private int _addCount = 3;

    public override void HandleClick()
    {
        _player.AddEnergy(_addCount);

        _window.Close();
        _levelState.IsWin.Value = null;
        _level.StartPlay();
    }
}
