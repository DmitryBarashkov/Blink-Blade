using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EnableBloodFlag
{
    private const string MenuPath = "EditorFlags/Blood/Enabled";
    private const string EnabledBlood = "EnabledBlood";

    [MenuItem(MenuPath)]
    private static void Toggle()
    {
        bool result = EditorPrefs.GetBool(EnabledBlood);

        EditorPrefs.SetBool(EnabledBlood, !result);
    }

    [MenuItem(MenuPath, true)]
    private static bool ToggleValidate()
    {
        Menu.SetChecked(MenuPath, EditorPrefs.GetBool(EnabledBlood));

        return true;
    }
}
