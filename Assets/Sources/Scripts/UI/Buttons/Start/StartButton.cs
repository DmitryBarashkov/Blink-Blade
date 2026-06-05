using UnityEngine;
using Zenject;

public class StartButton : UIButton
{
    [Inject] private BetweenLevelScreen _screen;
    [Inject] private Level _level;

    public override void HandleClick()
    {
        if (_screen != null)        
            _screen.Deactivate();

        _level.StartPlay();
    }
}
