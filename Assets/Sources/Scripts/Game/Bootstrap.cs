using UnityEngine.SceneManagement;
using Zenject;
using YG;

public class Bootstrap : IInitializable
{
    public void Initialize()
    {
        SceneManager.LoadScene(YG2.saves.level);
    }
}
