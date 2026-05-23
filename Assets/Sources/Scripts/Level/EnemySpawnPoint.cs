using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private EnemyDatabase _database;

    [HideInInspector] public string selectedEnemyName;

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
