using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using static WeaponDatabase;

public class WeaponItem : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Image _preview;

    private PlayerWeapon _weapon;
    private ShopScreen _screen;
    private IDisposable _toggleSubscription;

    public int WeaponId { get; private set; }

    private void OnDestroy()
    {
        _toggleSubscription?.Dispose();
    }

    public void Initialize(ShopScreen screen, PlayerWeapon weapon, bool isPurchased, bool isChosen)
    {
        _screen = screen;
        _weapon = weapon;
        WeaponId = weapon.id;
        
        InitializeItem(weapon, isPurchased, isChosen);
        InitializeToggleControl();
    }

    public void SetToggle(bool value)
    {
        _toggle.SetIsOnWithoutNotify(value);        
    }

    public void SetWeapon()
    {
        _screen.ChangeWeapon(_weapon.id);
    }

    public void UpdateAfterBuy()
    {
        InitializeItem(_weapon, true, true);
        SetWeapon();
    }

    private void InitializeItem(PlayerWeapon weapon, bool isPurchased, bool isChosen)
    {
        _toggle.gameObject.SetActive(isPurchased);
        SetToggle(isChosen);

        _buyButton.gameObject.SetActive(isPurchased == false);
        _preview.sprite = weapon.preview;
    }

    private void InitializeToggleControl()
    {
        _toggleSubscription?.Dispose();

        _toggleSubscription = 
            _toggle.gameObject.AddComponent<ObservablePointerClickTrigger>()
            .OnPointerClickAsObservable()
            .Subscribe(pointerEventData =>
            {
                if (_toggle.isOn == false)
                {
                    _toggle.SetIsOnWithoutNotify(true);
                }
                else
                {
                    SetWeapon();
                }
            })
            .AddTo(this);
    }
}
