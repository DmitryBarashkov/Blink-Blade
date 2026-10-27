using System.Collections.Generic;
using BlinkBlade.Game;
using BlinkBlade.Players;
using UniRx;
using UnityEngine;
using Zenject;

namespace BlinkBlade.UI
{
    public class ShopScreen : UIScreen
    {
        private readonly SerialDisposable ChangeCoinsSubscription = new SerialDisposable();
        private readonly SerialDisposable ChangeSkinSubscription = new SerialDisposable();
        private readonly SerialDisposable ChangeWeaponSubscription = new SerialDisposable();

        [SerializeField] private List<WeaponItem> _weaponPrefabs;
        [SerializeField] private List<SkinItem> _skinPrefabs;
        [SerializeField] private Transform _weaponsContainer;
        [SerializeField] private Transform _skinsContainer;

        [Inject] private PlayerStats _playerStats;

        private List<WeaponItem> _weapons = new ();
        private List<SkinItem> _skins = new ();

        private ShopService _service;
        private DiContainer _diContainer;

        private int _chosenWeaponItemId;
        private int _chosenSkinItemId;

        [Inject]
        public override void Construct(ShopService service, DiContainer container)
        {
            base.Construct(service, container);

            _service = service;
            _diContainer = container;
        }

        public override void Setup()
        {
            ChangeCoinsSubscription.Disposable = _playerStats.CurrentCoins.Skip(1).Subscribe((newCoins) =>
            {
                UpdateSkinItems();
                UpdateWeaponItems();
            });

            ChangeSkinSubscription.Disposable = _playerStats.CurrentSkinId.Skip(1).Subscribe((newId) =>
            {
                ChangeChosenSkinItem(newId);
                UpdateSkinItems();
            });

            ChangeWeaponSubscription.Disposable = _playerStats.CurrentWeaponId.Skip(1).Subscribe((newId) =>
            {
                ChangeChosenWeaponItem(newId);
                UpdateWeaponItems();
            });

            FillItems();

            _gameObject.SetActive(true);
        }

        public void Close() => _gameObject.SetActive(false);

        private void ChangeChosenWeaponItem(int id)
        {
            if (_chosenWeaponItemId == id)
                return;

            _service.ChangeChosenWeaponItem(id);
            UpdateWeaponList(id);

            _chosenWeaponItemId = id;
        }

        private void ChangeChosenSkinItem(int id)
        {
            if (_chosenSkinItemId == id)
                return;

            _service.ChangeChosenSkinItem(id);
            UpdateSkinList(id);

            _chosenSkinItemId = id;
        }

        private void FillItems()
        {
            if (_weapons.Count == 0)
                FillWeaponItems();
            else
                UpdateWeaponItems();

            if (_skins.Count == 0)
                FillSkinItems();
            else
                UpdateSkinItems();
        }

        private void UpdateWeaponList(int newId)
        {
            foreach (ShopItem item in _weapons)
            {
                if (item.Id == newId)
                    item.SetToggle(true);

                if (item.Id == _chosenWeaponItemId)
                    item.SetToggle(false);
            }
        }

        private void UpdateSkinList(int newId)
        {
            foreach (SkinItem item in _skins)
            {
                if (item.Id == newId)
                    item.SetToggle(true);

                if (item.Id == _chosenSkinItemId)
                    item.SetToggle(false);
            }
        }

        private void FillWeaponItems()
        {
            foreach (WeaponItem item in _weaponPrefabs)
            {
                WeaponItem weaponItem = _diContainer.InstantiatePrefabForComponent<WeaponItem>(item, _weaponsContainer);
                int id = weaponItem.Id;
                bool isChosen = weaponItem.IsChosen;

                if (isChosen)
                    _chosenWeaponItemId = id;

                _weapons.Add(weaponItem);
            }
        }

        private void FillSkinItems()
        {
            foreach (SkinItem item in _skinPrefabs)
            {
                SkinItem skinItem = _diContainer.InstantiatePrefabForComponent<SkinItem>(item, _skinsContainer);
                int id = skinItem.Id;
                bool isChosen = skinItem.IsChosen;

                if (isChosen)
                    _chosenSkinItemId = id;

                _skins.Add(skinItem);
            }
        }

        private void UpdateSkinItems()
        {
            foreach (SkinItem item in _skins)
            {
                item.UpdateItem();
            }
        }

        private void UpdateWeaponItems()
        {
            foreach (WeaponItem item in _weapons)
            {
                item.UpdateItem();
            }
        }

        private void OnDestroy()
        {
            ChangeCoinsSubscription.Dispose();
            ChangeSkinSubscription.Dispose();
            ChangeWeaponSubscription.Dispose();
        }
    }
}