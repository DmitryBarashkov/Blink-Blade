using System.Collections.Generic;
using YG;
using Zenject;

public class ShopService
{
    [Inject] private PlayerStats _playerStats;

    private readonly HashSet<int> _purchasedItemIds = new();

    public ShopService()
    {
        _purchasedItemIds.Clear();

        foreach (var item in YG2.saves.purchasedItemsIds)
        {
            _purchasedItemIds.Add(item);
        }
    }

    public bool IsItemPurchased(int id)
    {
        return _purchasedItemIds.Contains(id);
    }

    public bool IsItemChosen(int id)
    {
        return YG2.saves.weaponId == id;
    }

    public void PurchaseItem(int id)
    {
        if (_purchasedItemIds.Add(id))
        {
            YG2.saves.purchasedItemsIds.Add(id);
            YG2.SaveProgress();

            ChangeChosenItem(id);
        }
    }

    public void ChangeChosenItem(int id)
    {
        _playerStats.currentWeaponId.Value = id;
        
        YG2.saves.weaponId = id;
        YG2.SaveProgress();
    }

}
