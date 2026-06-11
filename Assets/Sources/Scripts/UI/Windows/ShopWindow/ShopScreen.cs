using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ShopScreen : UIScreen
{
    [SerializeField] private WeaponItem _itemPrefab;
    [SerializeField] private Transform _container;
    
    private WeaponDatabase _weaponDatabase;
    private ShopService _service;
    private DiContainer _diContainer;
    
    private readonly List<WeaponItem> _weaponItems = new();
    private int _chosenItemId;

    [Inject]
    public override void Construct(ShopService service, WeaponDatabase database, DiContainer container)
    {
        base.Construct(service, database, container);
        
        _weaponDatabase = database;
        _service = service;
        _diContainer = container;
    }

    public override void Setup()
    {
        UpdateItems();
        _gameObject.SetActive(true);
    }

    public void Close() => _gameObject.SetActive(false);

    public void ChangeWeapon(int id)
    {
        if (_chosenItemId == id)
            return;

        _service.ChangeChosenItem(id);

        UpdateList(id);

        _chosenItemId = id;
    }

    private void UpdateItems()
    {
        if (_weaponItems.Count == 0)
            FillWeaponItems();
    }

    private void UpdateList(int newId)
    {
        foreach (WeaponItem item in _weaponItems)
        {
            if (item.WeaponId == newId)
            {
                item.SetToggle(true);
            }                

            if (item.WeaponId == _chosenItemId)
                item.SetToggle(false);
        }
    }

    private void FillWeaponItems()
    {
        foreach (var item in _weaponDatabase.weapons)
        {
            WeaponItem weaponItem = _diContainer.InstantiatePrefabForComponent<WeaponItem>(_itemPrefab, _container);
            int id = item.id;
            bool isChosen = _service.IsItemChosen(id);

            if (isChosen)
                _chosenItemId = id;

            weaponItem.Initialize(this, item, _service.IsItemPurchased(id), isChosen);
            _weaponItems.Add(weaponItem);
        }
    }
}
