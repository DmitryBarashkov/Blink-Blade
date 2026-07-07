using System.Collections.Generic;

namespace YG
{
    public partial class SavesYG
    {
        // Player
        public int level = 0;
        public int energy = 5;
        public int coins = 0;

        public bool isFinishedGame = false;

        //Weapon
        public int weaponId = 0;

        // Ads
        public bool isAdsDisabled = false;

        // Options
        public bool isSoundOn = true;        

        //Shop
        public List<int> purchasedItemsIds = new List<int> { 0 };
    }
}
