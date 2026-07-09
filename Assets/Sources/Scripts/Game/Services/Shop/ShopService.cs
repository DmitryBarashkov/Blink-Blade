using System.Collections.Generic;
using YG;
using Zenject;

public class ShopService
{
    [Inject] private PlayerStats _playerStats;    

    private readonly HashSet<int> _purchasedWeaponItemIds = new();
    private readonly HashSet<int> _purchasedSkinItemIds = new();

    public ShopService()
    {
        _purchasedWeaponItemIds.Clear();
        _purchasedSkinItemIds.Clear();

        foreach (var item in YG2.saves.purchasedWeaponItemIds)
            _purchasedWeaponItemIds.Add(item);
        
        foreach (var item in YG2.saves.purchasedSkinItemsIds)
            _purchasedSkinItemIds.Add(item);
    }

    public bool IsWeapontemPurchased(int id)
    {
        return _purchasedWeaponItemIds.Contains(id);
    }

    public bool IsSkinItemPurchased(int id)
    {
        return _purchasedSkinItemIds.Contains(id);
    }

    public bool IsWeaponChosen(int id)
    {
        return YG2.saves.weaponId == id;
    }

    public bool IsSkinChosen(int id)
    {
        return YG2.saves.skinId == id;
    }

    public void PurchaseWeapon(int id)
    {
        if (_purchasedWeaponItemIds.Add(id))
        {
            YG2.saves.purchasedWeaponItemIds.Add(id);
            YG2.SaveProgress();

            ChangeChosenWeaponItem(id);
        }
    }

    public void PurchaseSkin(int id)
    {
        if (_purchasedSkinItemIds.Add(id))
        {
            YG2.saves.purchasedSkinItemsIds.Add(id);
            YG2.SaveProgress();

            ChangeChosenSkinItem(id);
        }
    }

    public void ChangeChosenWeaponItem(int id)
    {
        _playerStats.currentWeaponId.Value = id;
                
        YG2.saves.weaponId = id;
        YG2.SaveProgress();
    }

    public void ChangeChosenSkinItem(int id)
    {
        _playerStats.currentSkinId.Value = id;

        YG2.saves.skinId = id;
        YG2.SaveProgress();
    }

}
