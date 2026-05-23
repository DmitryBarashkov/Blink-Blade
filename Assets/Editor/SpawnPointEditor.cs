using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(EnemySpawnPoint))]
public class SpawnPointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnemySpawnPoint spawner = (target as EnemySpawnPoint);
        SerializedProperty enemyNameProp = serializedObject.FindProperty("selectedEnemyName");
        SerializedProperty dbProp = serializedObject.FindProperty("_database");
        EnemyDatabase db = dbProp.objectReferenceValue as EnemyDatabase;

        if (db == null || db.enemies == null || db.enemies.Count == 0)
        {
            EditorGUILayout.HelpBox("Назначьте Enemy Database, в которой есть хотя бы один сконструированный враг.", MessageType.Warning);
            return;
        }

        List<string> optionsList = new List<string>();
        int currentIndex = 0;

        for (int i = 0; i < db.enemies.Count; i++)
        {
            string name = string.IsNullOrEmpty(db.enemies[i].enemyName) ? $"Без имени (Элемент {i})" : db.enemies[i].enemyName;
            optionsList.Add(name);
            
            if (db.enemies[i].enemyName == enemyNameProp.stringValue)
            {
                currentIndex = i;
            }
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Настройки спавна", EditorStyles.boldLabel);

        int newIndex = EditorGUILayout.Popup("Враг для спавна", currentIndex, optionsList.ToArray());

        if (newIndex != currentIndex || string.IsNullOrEmpty(enemyNameProp.stringValue))
        {
            enemyNameProp.stringValue = db.enemies[newIndex].enemyName;
            serializedObject.ApplyModifiedProperties();
        }
    }
}
