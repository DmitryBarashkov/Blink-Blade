using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [HideInInspector] public EnemyDatabase Database;
    [HideInInspector] public string SelectedEnemyName;

    public EnemyDatabase EnemyDatabase
    {
        get
        {
#if UNITY_EDITOR
            if (Database == null && !Application.isPlaying)
            {
                string[] guids = UnityEditor.AssetDatabase.FindAssets("t:EnemyDatabase");

                if (guids.Length > 0)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);

                    Database = UnityEditor.AssetDatabase.LoadAssetAtPath<EnemyDatabase>(path);
                }
            }
#endif
            return Database;
        }
    }

    private void Reset()
    {
#if UNITY_EDITOR

        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:EnemyDatabase");

        if (guids.Length > 0)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            Database = UnityEditor.AssetDatabase.LoadAssetAtPath<EnemyDatabase>(path);
        }
#endif
    }
}
