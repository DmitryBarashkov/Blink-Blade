using System.Collections.Generic;
using BlinkBlade.Players;
using BlinkBlade.UI;
using YG;
using Zenject;

namespace BlinkBlade.Game
{
    public class ShopService
    {
        private readonly HashSet<int> PurchasedWeaponItemIds = new ();
        private readonly HashSet<int> PurchasedSkinItemIds = new ();

        [Inject] private PlayerStats _playerStats;

        public ShopService()
        {
            PurchasedWeaponItemIds.Clear();
            PurchasedSkinItemIds.Clear();

            foreach (var item in YG2.saves.PurchasedWeaponItemIds)
                PurchasedWeaponItemIds.Add(item);

            foreach (var item in YG2.saves.PurchasedSkinItemsIds)
                PurchasedSkinItemIds.Add(item);
        }

        public bool IsWeaponItemPurchased(int id)
        {
            return PurchasedWeaponItemIds.Contains(id);
        }

        public bool IsSkinItemPurchased(int id)
        {
            return PurchasedSkinItemIds.Contains(id);
        }

        public bool IsWeaponChosen(int id)
        {
            return YG2.saves.WeaponId == id;
        }

        public bool IsSkinChosen(int id)
        {
            return YG2.saves.SkinId == id;
        }

        public void PurchaseItem(ShopItem item)
        {
            if (item is WeaponItem)
            {
                YG2.saves.PurchasedWeaponItemIds.Add(item.Id);
                PurchasedWeaponItemIds.Add(item.Id);
                YG2.SaveProgress();

                ChangeChosenWeaponItem(item.Id);
            }

            if (item is SkinItem)
            {
                YG2.saves.PurchasedSkinItemsIds.Add(item.Id);
                PurchasedSkinItemIds.Add(item.Id);
                YG2.SaveProgress();

                ChangeChosenSkinItem(item.Id);
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
}