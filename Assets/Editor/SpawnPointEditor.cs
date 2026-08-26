using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(EnemySpawnPoint))]
public class SpawnPointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnemySpawnPoint spawner = (target as EnemySpawnPoint);
        SerializedProperty enemyNameProp = serializedObject.FindProperty("SelectedEnemyName");
        SerializedProperty dbProp = serializedObject.FindProperty("Database");
        EnemyDatabase db = spawner.EnemyDatabase;

        if (db == null || db.Enemies == null || db.Enemies.Count == 0)
        {
            EditorGUILayout.HelpBox("Enemy Database is empty", MessageType.Warning);
            return;
        }

        List<string> optionsList = new List<string>();
        int currentIndex = 0;

        for (int i = 0; i < db.Enemies.Count; i++)
        {
            string name = string.IsNullOrEmpty(db.Enemies[i].EnemyName) ? $"Noname (Element {i})" : db.Enemies[i].EnemyName;
            optionsList.Add(name);
            
            if (db.Enemies[i].EnemyName == enemyNameProp.stringValue)
            {
                currentIndex = i;
            }
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Spawn Settings", EditorStyles.boldLabel);

        int newIndex = EditorGUILayout.Popup("Enemy", currentIndex, optionsList.ToArray());

        if (newIndex != currentIndex || string.IsNullOrEmpty(enemyNameProp.stringValue))
        {
            enemyNameProp.stringValue = db.Enemies[newIndex].EnemyName;
            serializedObject.ApplyModifiedProperties();
        }
    }
}
