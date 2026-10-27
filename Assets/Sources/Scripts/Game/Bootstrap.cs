using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
using Zenject;

namespace BlinkBlade.Game
{
    public class Bootstrap : IInitializable
    {
        [Inject] private LevelLoadService _levelService;

        public void Initialize()
        {
            CheckInAppPurchases();

            if (YG2.saves.IsAdsDisabled)
                YG2.StickyAdActivity(false);

            if (SceneManager.GetActiveScene().buildIndex != 0)
                return;

            _levelService.LoadLevel(YG2.saves.Level).Forget();
        }

        private void CheckInAppPurchases()
        {
            foreach (var purchase in YG2.purchases)
            {
                if (purchase.consumed == false)
                    GetAward(purchase.id);
            }
        }

        private void GetAward(string id)
        {
            if (id == "no_ads")
            {
                Debug.LogError("Consumed ads");

                YG2.saves.IsAdsDisabled = true;
                YG2.SaveProgress();
            }
        }
    }
}