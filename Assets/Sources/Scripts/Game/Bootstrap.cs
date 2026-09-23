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

            StartLevel();
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

        private void StartLevel()
        {
            int levelNumber = YG2.saves.Level;

            if (levelNumber == 0)
                _levelService.LoadTutorialLevel();
            else
                _levelService.LoadLevel(levelNumber).Forget();
        }
    }
}