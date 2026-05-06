using Zenject;

public class StartButton : UIButton
{
    [Inject] private Level _level;

    public override void HandleClick()
    {
        gameObject.SetActive(false);

        _level.StartPlay();
    }
}
