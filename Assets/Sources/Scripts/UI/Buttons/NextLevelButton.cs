using UnityEngine.SceneManagement;
using YG;
using Zenject;

public class NextLevelButton : UIButton
{
    [Inject] private LevelLoadService _levelService;

    public override void HandleClick()
    {
        if (YG2.saves.IsAdsDisabled == false)
            YG2.InterstitialAdvShow();

        SceneManager.LoadScene(_levelService.GetSceneName(YG2.saves.level));
    }
}
