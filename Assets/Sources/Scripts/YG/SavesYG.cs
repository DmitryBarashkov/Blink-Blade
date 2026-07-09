using System.Collections.Generic;

namespace YG
{
    public partial class SavesYG
    {
        // Player
        public int level = 0;
        public int energy = 5;
        public int coins = 0;
        public int rating = 0;

        public bool isFinishedGame = false;

        //Items
        public int weaponId = 0;
        public int skinId = 0;

        // Ads
        public bool isAdsDisabled = false;

        // Options
        public bool isSoundOn = true;        

        //Shop
        public List<int> purchasedWeaponItemIds = new List<int> { 0 };
        public List<int> purchasedSkinItemsIds = new List<int> { 0 };
    }
}
