#if UNITY_EDITOR
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using YG;


[InitializeOnLoad]
public static class StartSceneBootstrapper
{
    private const string StartScenePath = "Assets/Sources/Scenes/Service/Loading.unity";
    private const string PhotoScenePath = "Assets/Sources/Scenes/Service/Photo.unity";

    static StartSceneBootstrapper()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            string currentScene = EditorSceneManager.GetActiveScene().path;

            if (currentScene == StartScenePath || currentScene == PhotoScenePath) return;

            int levelNumber = GetLevelNumberFromName(EditorSceneManager.GetActiveScene().name);

            YG2.saves.Level = levelNumber;
            YG2.SaveProgress();

            EditorPrefs.SetString("LeftScenePath", currentScene);
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(StartScenePath);

        }

        if (state == PlayModeStateChange.EnteredEditMode)
        {
            if (EditorPrefs.HasKey("LeftScenePath"))
            {
                string lastScene = EditorPrefs.GetString("LeftScenePath");

                EditorSceneManager.OpenScene(lastScene);
                EditorPrefs.DeleteKey("LeftScenePath");
            }
        }
    }

    private static int GetLevelNumberFromName(string sceneName)
    {
        Match match = Regex.Match(sceneName, @"\d+");

        if (match.Success)
        {
            return int.Parse(match.Value);
        }

        return 0;
    }
}
#endif