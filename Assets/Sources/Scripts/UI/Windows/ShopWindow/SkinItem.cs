using System;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using YG;
using static SkinDatabase;

public class SkinItem : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _buyButtonText;
    [SerializeField] private Button _backgroundButton;
    [SerializeField] private Image _preview;

    private PlayerSkin _skin;
    private ShopScreen _screen;
    private IDisposable _toggleSubscription;

    public int SkinId { get; private set; }

    public int Cost { get; private set; }

    private void OnDestroy()
    {
        _toggleSubscription?.Dispose();
    }

    public void Initialize(ShopScreen screen, PlayerSkin skin, bool isPurchased, bool isChosen)
    {
        _screen = screen;
        _skin = skin;
        _buyButtonText.text = skin.Cost.ToString();
        SkinId = skin.Id;
        Cost = skin.Cost;

        InitializeItem(skin, isPurchased, isChosen);
        InitializeToggleControl();
    }

    public void SetToggle(bool value)
    {
        _toggle.SetIsOnWithoutNotify(value);
    }

    public void SetSkin()
    {
        _screen.ChangeChosenSkinItem(_skin.Id);
    }

    public void UpdateAfterBuy()
    {
        InitializeItem(_skin, true, true);
        SetSkin();
    }

    private void InitializeItem(PlayerSkin skin, bool isPurchased, bool isChosen)
    {
        _toggle.gameObject.SetActive(isPurchased);
        SetToggle(isChosen);

        _buyButton.gameObject.SetActive(isPurchased == false);
        _buyButton.interactable = YG2.saves.Coins >= skin.Cost;
        _preview.sprite = skin.Preview;
        _backgroundButton.interactable = isPurchased == true;
    }

    private void InitializeToggleControl()
    {
        if (_toggle.gameObject.GetComponent<ObservablePointerClickTrigger>())
            return;

        _toggleSubscription?.Dispose();

        _toggleSubscription =
            _toggle.gameObject.AddComponent<ObservablePointerClickTrigger>()
            .OnPointerClickAsObservable()
            .Subscribe(pointerEventData =>
            {
                if (_toggle.isOn == false)
                    _toggle.SetIsOnWithoutNotify(true);
                else
                    SetSkin();
            })
            .AddTo(this);
    }
}
