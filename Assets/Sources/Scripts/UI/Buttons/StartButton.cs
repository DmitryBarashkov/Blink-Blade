using UnityEngine;
using Zenject;

public class StartButton : UIButton
{
    [SerializeField] private RectTransform _screen;
    
    [Inject] private Level _level;

    public override void HandleClick()
    {
        _screen.gameObject.SetActive(false);
        _level.StartPlay();
    }
}
