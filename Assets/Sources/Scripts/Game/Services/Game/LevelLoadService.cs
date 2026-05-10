using UnityEngine.SceneManagement;

public class LevelLoadService
{
    private int _noRepeatLevels = 2;
    
    public string GetSceneName(int levelForLoad)
    {
        int levelCount = SceneManager.sceneCountInBuildSettings - _noRepeatLevels;        

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
}
