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
            await LoadCommonScene();
            await UnloadOldLevel();
            await LoadNewLevel(levelName);
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

    private async UniTask LoadCommonScene()
    {
        Scene commonScene = SceneManager.GetSceneByName(_commonSceneName);

        if (commonScene.isLoaded)
            return;

        var tcs = new TaskCompletionSource<bool>();

        await _sceneLoader.LoadSceneAsync(_commonSceneIndex, LoadSceneMode.Single).ToUniTask();
    }

    private async UniTask UnloadOldLevel()
    {
        if (string.IsNullOrEmpty(_currentLevelName))
            return;

        Scene oldScene = SceneManager.GetSceneByName(_currentLevelName);
        
        if (!oldScene.isLoaded)
            return;

        await SceneManager.UnloadSceneAsync(_currentLevelName);
        await Resources.UnloadUnusedAssets();
    }

    private async UniTask LoadNewLevel(string levelName)
    {
        _currentLevelName = levelName;

        await _sceneLoader.LoadSceneAsync(levelName, LoadSceneMode.Additive);

        Scene loadedScene = SceneManager.GetSceneByName(levelName);

        if (loadedScene.IsValid())
        {
            SceneManager.SetActiveScene(loadedScene);
        }

        ExecuteFinalSceneSetup(levelName);
    }

    private async void ExecuteFinalSceneSetup(string levelName)
    {
        await Task.Yield();

        Scene loadedScene = SceneManager.GetSceneByName(levelName);
            
        if (loadedScene.IsValid())
        {
            SceneManager.SetActiveScene(loadedScene);            
        }            
    }

    private void LoadSingleScene(string levelName)
    {
        _currentLevelName = levelName;

        _sceneLoader.LoadSceneAsync(levelName, LoadSceneMode.Single, container =>
        {
            Scene loadedScene = SceneManager.GetSceneByName(levelName);

            if (loadedScene.IsValid())
            {
                Observable.NextFrame()
                    .Take(1)
                    .Subscribe(_ =>
                    {
                        SceneManager.SetActiveScene(loadedScene);                        
                    });
            }
        });
    }
}
