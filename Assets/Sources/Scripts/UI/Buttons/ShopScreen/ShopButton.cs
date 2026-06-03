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
        _uiService.ShowShop();
    }
}
