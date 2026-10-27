using UnityEditor;

public static class RestartLevelFlag
{
    private const string MenuPath = "EditorFlags/RestartLevelAfterFinish/Enabled";
    private const string RestartAfterFinish = "RestartAfterFinish";

    [MenuItem(MenuPath)]
    private static void Toggle()
    {
        bool result = EditorPrefs.GetBool(RestartAfterFinish);

        EditorPrefs.SetBool(RestartAfterFinish, !result);
    }

    [MenuItem(MenuPath, true)]
    private static bool ToggleValidate()
    {
        Menu.SetChecked(MenuPath, EditorPrefs.GetBool(RestartAfterFinish));
        
        return true;
    }
}


