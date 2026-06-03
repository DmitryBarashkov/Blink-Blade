using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShopGameScreen : UIScreen
{
    [SerializeField] private WeaponDatabase _weaponDatabase;
    [SerializeField] private WeaponItem _itemPrefab;
    [SerializeField] private Transform _container;
    
    private GameObject _gameObject;
    private readonly List<WeaponItem> _weaponItems = new();

    private void Awake()
    {
        _gameObject = gameObject;
    }

    public override void Setup()
    {
        UpdateItems();
        _gameObject.SetActive(true);
    }

    public void Close() => _gameObject.SetActive(false);

    private void UpdateItems()
    {
        if (_weaponItems.Count == 0)
            FillWeaponItems();
    }

    private void FillWeaponItems()
    {
        foreach (var item in _weaponDatabase.weapons)
        {
            WeaponItem weaponItem = Instantiate(_itemPrefab, _container);

            weaponItem.Initialize(item);
            _weaponItems.Add(weaponItem);
        }
    }
}
