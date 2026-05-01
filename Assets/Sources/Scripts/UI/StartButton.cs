using Zenject;

public class StartButton : MenuButton
{
    [Inject] private Level _level;

    public override void HandleClick()
    {
        gameObject.SetActive(false);

        _level.StartPlay();
    }
}
