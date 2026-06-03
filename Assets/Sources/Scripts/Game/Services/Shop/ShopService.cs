using System.Collections.Generic;
using YG;

public class ShopService
{
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

    public void PurchaseItem(int id)
    {
        if (_purchasedItemIds.Add(id))
        {
            YG2.saves.purchasedItemsIds.Add(id);
            YG2.SaveProgress();
        }
    }
}
