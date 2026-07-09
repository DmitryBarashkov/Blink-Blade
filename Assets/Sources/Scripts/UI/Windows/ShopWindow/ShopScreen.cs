using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ShopScreen : UIScreen
{
    [SerializeField] private WeaponItem _weaponItemPrefab;
    [SerializeField] private SkinItem _skinItemPrefab;
    [SerializeField] private Transform _weaponsContainer;
    [SerializeField] private Transform _skinsContainer;
    
    private WeaponDatabase _weaponDatabase;
    private SkinDatabase _skinDatabase;
    private ShopService _service;
    private DiContainer _diContainer;
    
    private readonly List<WeaponItem> _weaponItems = new();
    private readonly List<SkinItem> _skinItems = new();
    private int _chosenWeaponItemId;
    private int _chosenSkinItemId;

    [Inject]
    public override void Construct(ShopService service, WeaponDatabase weaponDatabase, SkinDatabase skinDatabase, DiContainer container)
    {
        base.Construct(service, weaponDatabase, skinDatabase, container);

        _skinDatabase = skinDatabase;
        _weaponDatabase = weaponDatabase;
        _service = service;
        _diContainer = container;
    }

    public override void Setup()
    {
        UpdateItems();
        _gameObject.SetActive(true);
    }

    public void Close() => _gameObject.SetActive(false);

    public void ChangeChosenWeaponItem(int id)
    {
        if (_chosenWeaponItemId == id)
            return;

        _service.ChangeChosenWeaponItem(id);
        UpdateWeaponList(id);

        _chosenWeaponItemId = id;
    }

    public void ChangeChosenSkinItem(int id)
    {
        if (_chosenSkinItemId == id)
            return;

        _service.ChangeChosenSkinItem(id);
        UpdateSkinList(id);

        _chosenSkinItemId = id;
    }

    private void UpdateItems()
    {
        if (_weaponItems.Count == 0)
            FillWeaponItems();

        if (_skinItems.Count == 0)
            FillSkinsItems();
        else
        {
            UpdateSkinsItems();
        }
    }

    private void UpdateWeaponList(int newId)
    {
        foreach (WeaponItem item in _weaponItems)
        {
            if (item.WeaponId == newId)
                item.SetToggle(true);
            
            if (item.WeaponId == _chosenWeaponItemId)
                item.SetToggle(false);
        }
    }

    private void UpdateSkinList(int newId)
    {
        foreach (SkinItem item in _skinItems)
        {
            if (item.SkinId == newId)
                item.SetToggle(true);            
            
            if (item.SkinId == _chosenSkinItemId)
                item.SetToggle(false);
        }
    }

    private void FillWeaponItems()
    {
        foreach (var item in _weaponDatabase.weapons)
        {
            WeaponItem weaponItem = _diContainer.InstantiatePrefabForComponent<WeaponItem>(_weaponItemPrefab, _weaponsContainer);
            int id = item.id;
            bool isChosen = _service.IsWeaponChosen(id);

            if (isChosen)
                _chosenWeaponItemId = id;

            weaponItem.Initialize(this, item, _service.IsWeapontemPurchased(id), isChosen);
            _weaponItems.Add(weaponItem);
        }
    }

    private void FillSkinsItems()
    {
        foreach (var item in _skinDatabase.skins)
        {
            SkinItem skinItem = _diContainer.InstantiatePrefabForComponent<SkinItem>(_skinItemPrefab, _skinsContainer);
            int id = item.id;
            bool isChosen = _service.IsSkinChosen(id);

            if (isChosen)
                _chosenSkinItemId = id;

            skinItem.Initialize(this, item, _service.IsSkinItemPurchased(id), isChosen);
            _skinItems.Add(skinItem);
        }
    }

    private void UpdateSkinsItems()
    {
        foreach (var item in _skinDatabase.skins)
        {
            int id = item.id;
            bool isChosen = _service.IsSkinChosen(id);
            bool isPurchased = _service.IsSkinItemPurchased(id);

            if (isPurchased == false)
            {
                _skinItems[id].Initialize(this, item, isPurchased, isChosen);
            }
        }
    }
}
