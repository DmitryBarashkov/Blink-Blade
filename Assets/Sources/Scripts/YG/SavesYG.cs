using System.Collections.Generic;

namespace YG
{
    public partial class SavesYG
    {
        // Player
        public int level = 1;
        public int energy = 50;
        public int coins = 0;

        //Weapon
        public int weaponId = 0;

        // Ads
        public bool isAdsDisabled = false;

        // Options
        public bool isSoundOn = true;
        public bool isMusicOn = true;

        //Shop
        public List<int> purchasedItemsIds = new List<int> { 0 };
    }
}
