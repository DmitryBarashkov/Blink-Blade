using UnityEngine.SceneManagement;
using Zenject;
using YG;

public class Bootstrap : IInitializable
{
    [Inject] private LevelLoadService _levelService;    
    
    public void Initialize()
    {
        if (YG2.saves.isAdsDisabled)
            YG2.StickyAdActivity(false);

        if (SceneManager.GetActiveScene().buildIndex != 0)
            return;

        StartLevel();
    }

    private void StartLevel()
    {
        int levelnumber = YG2.saves.level;

        if (levelnumber == 0)
            _levelService.LoadTutorialLevel();
        else
            _levelService.LoadLevel(levelnumber).Forget();
    }
}
