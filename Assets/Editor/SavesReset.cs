using UnityEditor;
using UnityEngine;
using YG;

[InitializeOnLoad]
public class ResetYGOnPlay
{
    static ResetYGOnPlay()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        //if (state == PlayModeStateChange.ExitingEditMode)
        //{
        //    string path = Application.dataPath + "/PluginYourGames/Editor/SavesEditorYG2.json";

        //    if (System.IO.File.Exists(path))
        //    {
        //        System.IO.File.Delete(path);
        //    }
        //}
    }
}
