using System.Collections.Generic;

namespace YG
{
    public partial class SavesYG
    {
        // Player
        public int Level = 0;
        public int Energy = 5;
        public int Coins = 0;
        public int Rating = 0;

        public bool IsFinishedGame = false;

        // Items
        public int WeaponId = 0;
        public int SkinId = 0;

        // Ads
        public bool IsAdsDisabled = false;

        // Options
        public bool IsSoundOn = true;

        // Shop
        public List<int> PurchasedWeaponItemIds = new List<int> { 0 };
        public List<int> PurchasedSkinItemsIds = new List<int> { 0 };
    }
}
