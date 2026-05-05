using UnityEngine.SceneManagement;
using Zenject;
using YG;
using Cysharp.Threading.Tasks;

public class Bootstrap : IInitializable
{
    public async void Initialize()
    {
        //await UniTask.Delay(5000);
        await StartLevel();
    }

    private async UniTask StartLevel()
    {
        await SceneManager.LoadSceneAsync($"Level{YG2.saves.level}");
    }
}
