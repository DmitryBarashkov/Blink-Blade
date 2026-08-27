using System.Collections.Generic;
using YG;
using Zenject;

public class ShopService
{
    private readonly HashSet<int> _purchasedWeaponItemIds = new ();
    private readonly HashSet<int> _purchasedSkinItemIds = new ();

    [Inject] private PlayerStats _playerStats;

    public ShopService()
    {
        _purchasedWeaponItemIds.Clear();
        _purchasedSkinItemIds.Clear();

        foreach (var item in YG2.saves.PurchasedWeaponItemIds)
            _purchasedWeaponItemIds.Add(item);

        foreach (var item in YG2.saves.PurchasedSkinItemsIds)
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
        return YG2.saves.WeaponId == id;
    }

    public bool IsSkinChosen(int id)
    {
        return YG2.saves.SkinId == id;
    }

    public void PurchaseWeapon(int id)
    {
        if (_purchasedWeaponItemIds.Add(id))
        {
            YG2.saves.PurchasedWeaponItemIds.Add(id);
            YG2.SaveProgress();

            ChangeChosenWeaponItem(id);
        }
    }

    public void PurchaseSkin(int id)
    {
        if (_purchasedSkinItemIds.Add(id))
        {
            YG2.saves.PurchasedSkinItemsIds.Add(id);
            YG2.SaveProgress();

            ChangeChosenSkinItem(id);
        }
    }

    public void ChangeChosenWeaponItem(int id)
    {
        _playerStats.CurrentWeaponId.Value = id;

        YG2.saves.WeaponId = id;
        YG2.SaveProgress();
    }

    public void ChangeChosenSkinItem(int id)
    {
        _playerStats.CurrentSkinId.Value = id;

        YG2.saves.SkinId = id;
        YG2.SaveProgress();
    }
}