using UnityEngine;
using Zenject;

public class RestartButton : UIButton
{
    [SerializeField] EndGameWindow _window;
    
    [Inject] Level _level;
    
    public override void HandleClick()
    {
        _window.Close();
        _level.Restart();
    }
}
