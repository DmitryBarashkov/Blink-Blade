using YG;

public class NoAdsButton : UIButton
{
    private string _buyId = "no_ads";

    private void OnEnable() => YG2.onPurchaseSuccess += OnSuccess;
    private void OnDisable() => YG2.onPurchaseSuccess -= OnSuccess;

    public override void HandleClick()
    {
        YG2.BuyPayments(_buyId);
    }

    private void OnSuccess(string id)
    {
        if (id == "no_ads")
        {
            YG2.saves.IsAdsDisabled = true;
            YG2.StickyAdActivity(false);
            YG2.SaveProgress();
        }
    }
}
