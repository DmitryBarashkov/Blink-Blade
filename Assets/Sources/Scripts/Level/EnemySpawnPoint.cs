using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private EnemyDatabase _database;

    [HideInInspector] public bool isPatrolList;
    [HideInInspector] public int selectedEnemyIndex;

    public Enemy EnemyPrefab { get; private set; }

    private void Awake()
    {
        if (isPatrolList)
        {
            if (selectedEnemyIndex >= 0 && selectedEnemyIndex < _database.patrolEnemies.Count)
                EnemyPrefab = _database.patrolEnemies[selectedEnemyIndex].prefab;
        }
        else
        {
            if (selectedEnemyIndex >= 0 && selectedEnemyIndex < _database.idleEnemies.Count)
                EnemyPrefab = _database.idleEnemies[selectedEnemyIndex].prefab;
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
