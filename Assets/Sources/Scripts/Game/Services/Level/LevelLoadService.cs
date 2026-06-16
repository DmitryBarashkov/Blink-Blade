using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LevelLoadService
{
    private readonly ZenjectSceneLoader _sceneLoader;    

    private string _commonSceneName = "Common";
    private string _loadingSceneName = "Loading";

    private int _loadingSceneIndex = 0;
    private int _commonSceneIndex = 1;
    
    private string _currentLevelName;

    private int _noRepeatLevels = 3;
        
    public LevelLoadService(ZenjectSceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;        
    }

    public async UniTaskVoid LoadLevel(int levelNumber)
    {
        string levelName = GetSceneName(levelNumber);

        try
        {
            await LoadLoadingScreen();
            await LoadCommonScene();
            await UnloadScene(_currentLevelName);
            await LoadNewLevel(levelName);
            await UnloadScene(_loadingSceneName);
        }
        catch (Exception exception)
        {
            Debug.LogError($"--- [ASYNC LOADER] КРИТИЧЕСКАЯ ОШИБКА В ЦЕПОЧКЕ: {exception.Message}\n{exception.StackTrace}");
        }
    }

    public void LoadTutorialLevel()
    {
        LoadSingleScene(GetSceneName(0));
    }

    private async UniTask LoadLoadingScreen()
    {
        Scene loadingScene = SceneManager.GetSceneByName(_loadingSceneName);

        if (loadingScene.isLoaded)
            return;

        await _sceneLoader.LoadSceneAsync(_loadingSceneIndex, LoadSceneMode.Additive).ToUniTask();

        Scene loadedScene = SceneManager.GetSceneByName(_loadingSceneName);

        if (loadedScene.IsValid())
        {
            SceneManager.SetActiveScene(loadedScene);
        }
    }

    private async UniTask LoadCommonScene()
    {
        Scene commonScene = SceneManager.GetSceneByName(_commonSceneName);

        if (commonScene.isLoaded)
            return;

        var tcs = new TaskCompletionSource<bool>();

        await _sceneLoader.LoadSceneAsync(_commonSceneIndex, LoadSceneMode.Additive).ToUniTask();
    }

    private async UniTask UnloadScene(string levelName)
    {
        if (string.IsNullOrEmpty(levelName))
            return;

        Scene sceneForUnload = SceneManager.GetSceneByName(levelName);
        
        if (!sceneForUnload.isLoaded)
            return;

        await SceneManager.UnloadSceneAsync(levelName);
        await Resources.UnloadUnusedAssets();
    }

    private async UniTask LoadNewLevel(string levelName)
    {
        _currentLevelName = levelName;

        await _sceneLoader.LoadSceneAsync(levelName, LoadSceneMode.Additive);

        ActivateScene(levelName);
        ExecuteFinalSceneSetup(levelName);
    }

    private async void ExecuteFinalSceneSetup(string levelName)
    {
        await Task.Yield();

        ActivateScene(levelName);
    }

    private void LoadSingleScene(string levelName)
    {
        _currentLevelName = levelName;

        _sceneLoader.LoadSceneAsync(levelName, LoadSceneMode.Single, container =>
        {
            ActivateScene(levelName);
        });
    }

    private string GetSceneName(int levelForLoad)
    {
        int levelCount = SceneManager.sceneCountInBuildSettings - _noRepeatLevels;

        if (levelCount == 0)
            throw new ArgumentNullException("Not enough Levels in game");

        if (levelForLoad == 0)
            return "Level0";

        if (levelForLoad > levelCount)
        {
            int levelNumber = levelForLoad - levelCount;

            while (levelNumber > levelCount)
                levelNumber -= levelCount;

            return $"Level{levelNumber}";
        }


        return $"Level{levelForLoad}";
    }

    private void ActivateScene(string sceneName)
    {
        Scene loadedScene = SceneManager.GetSceneByName(sceneName);

        if (loadedScene.IsValid() && loadedScene.isLoaded)
            SceneManager.SetActiveScene(loadedScene);
    }
}


