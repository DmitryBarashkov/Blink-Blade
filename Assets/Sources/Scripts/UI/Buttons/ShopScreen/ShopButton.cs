using Zenject;

public class ShopButton : UIButton
{
    private UIService _uiService;

    [Inject]
    public void Construct(UIService uiService)
    {
        _uiService = uiService;
    }

    public override void HandleClick()
    {
        _audioService.PlaySound(SoundType.ButtonClick);
        _uiService.ShowShop();
    }
}
