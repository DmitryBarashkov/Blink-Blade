using UnityEngine;

public class LanguageButton : UIButton
{
    [SerializeField] private LanguagePanel _panel;

    public override void HandleClick()
    {
        _panel.ToggleMenu();
    }
}
