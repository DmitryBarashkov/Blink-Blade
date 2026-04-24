using System;

public class StartButton : MenuButton
{
    public event Action StartLevel;
    
    public override void HandleClick()
    {
        gameObject.SetActive(false);

        StartLevel?.Invoke();
    }
}
