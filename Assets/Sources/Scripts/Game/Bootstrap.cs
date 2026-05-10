using UnityEngine.SceneManagement;
using Zenject;
using YG;
using Cysharp.Threading.Tasks;

public class Bootstrap : IInitializable
{
    [Inject] private LevelLoadService _levelService;
    
    public async void Initialize()
    {
        if (YG2.saves.IsAdsDisabled)
            YG2.StickyAdActivity(false);

        if (SceneManager.GetActiveScene().buildIndex != 0)
            return;

        await StartLevel();
    }

    private async UniTask StartLevel()
    {
        await SceneManager.LoadSceneAsync(_levelService.GetSceneName(YG2.saves.level));
    }
}
