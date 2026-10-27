using BlinkBlade.Game;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

namespace BlinkBlade.UI
{
    public abstract class ShopItem : MonoBehaviour
    {
        [SerializeField] protected PlayerEquipment _equip;
        [SerializeField] protected TextMeshProUGUI _buyButtonText;

        protected ShopService _shopService;

        private readonly SerialDisposable _toggleSubscription = new SerialDisposable();

        [SerializeField] private Toggle _toggle;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _backgroundButton;
        [SerializeField] private Image _preview;

        public int Id { get; protected set; }

        public int Cost { get; protected set; }

        public bool IsChosen { get; protected set; }

        public bool IsPurchased { get; protected set; }

        [Inject] public abstract void Construct(ShopService shopService);

        public void SetToggle(bool value)
        {
            _toggle.SetIsOnWithoutNotify(value);
        }

        public abstract void UpdateItem();

        public void UpdateAfterBuy()
        {
            IsChosen = true;
            IsPurchased = true;

            InitializeItem();
        }

        protected void InitializeItem()
        {
            _toggle.gameObject.SetActive(IsPurchased);
            SetToggle(IsChosen);

            _buyButton.gameObject.SetActive(IsPurchased == false);
            _buyButton.interactable = YG2.saves.Coins >= _equip.Cost;
            _preview.sprite = _equip.Preview;
            _backgroundButton.interactable = IsPurchased == true;
        }

        protected void InitializeToggleControl()
        {
            if (_toggle.gameObject.GetComponent<ObservablePointerClickTrigger>())
                return;

            _toggleSubscription.Disposable =
                _toggle.gameObject.AddComponent<ObservablePointerClickTrigger>()
                .OnPointerClickAsObservable()
                .Subscribe(pointerEventData =>
                {
                    if (_toggle.isOn == false)
                        _toggle.SetIsOnWithoutNotify(true);
                });
        }

        private void OnDestroy()
        {
            _toggleSubscription.Dispose();
        }
    }
}
