using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [HideInInspector] public EnemyDatabase _database;
    [HideInInspector] public string selectedEnemyName;

    public EnemyDatabase Database
    {
        get
        {
            
#if UNITY_EDITOR
            if (_database == null && !Application.isPlaying)
            {
                string[] guids = UnityEditor.AssetDatabase.FindAssets("t:EnemyDatabase");
                if (guids.Length > 0)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                    _database = UnityEditor.AssetDatabase.LoadAssetAtPath<EnemyDatabase>(path);
                }
            }
#endif
            return _database;
        }
    }

    private void Reset()
    {
#if UNITY_EDITOR
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:EnemyDatabase");
        if (guids.Length > 0)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            _database = UnityEditor.AssetDatabase.LoadAssetAtPath<EnemyDatabase>(path);
        }
#endif
    }
}
