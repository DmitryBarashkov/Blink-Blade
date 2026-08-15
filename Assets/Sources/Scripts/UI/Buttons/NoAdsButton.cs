using YG;
using UnityEngine;

public class NoAdsButton : UIButton
{
    private string _buyId = "no_ads";

    protected override void OnEnable()
    {
        _button.onClick.AddListener(HandleClick);
        YG2.onPurchaseSuccess += OnSuccess;
    }

    protected override void OnDisable()
    {
        _button.onClick.AddListener(HandleClick);
        YG2.onPurchaseSuccess -= OnSuccess;
    }

    public override void HandleClick()
    {
        _audioService.PlaySound(SoundType.ButtonClick);
        
        YG2.BuyPayments(_buyId);
    }

    private void OnSuccess(string id)
    {
        if (id == "no_ads")
        {
            YG2.saves.isAdsDisabled = true;
            YG2.StickyAdActivity(false);
            YG2.SaveProgress();

            gameObject.SetActive(false);
        }
    }
}
