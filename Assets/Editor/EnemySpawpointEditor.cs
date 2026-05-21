#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(EnemySpawnPoint))]
public class EnemySpawpointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnemySpawnPoint spawner = (EnemySpawnPoint)target;

        SerializedProperty databaseProp = serializedObject.FindProperty("_database");
        EnemyDatabase db = databaseProp.objectReferenceValue as EnemyDatabase;

        if (db == null)
        {
            EditorGUILayout.HelpBox("Пожалуйста, укажите ассет EnemyDatabase в поле выше!", MessageType.Warning);
            return;
        }

        if (db.idleEnemies == null && db.patrolEnemies == null || db.idleEnemies.Count == 0 && db.patrolEnemies == null)
        {
            EditorGUILayout.HelpBox("База данных пуста! Добавьте префабы в ассет EnemyDatabase.", MessageType.Info);
            return;
        }

        List<string> displayNames = new List<string>();
        List<bool> listIdentities = new List<bool>();
        List<int> localIndices = new List<int>();

        if (db.idleEnemies != null)
        {
            for (int i = 0; i < db.patrolEnemies.Count; i++)
            {
                displayNames.Add(db.idleEnemies[i].behaviour.ToString() + " " + db.idleEnemies[i].type.ToString());
                listIdentities.Add(false);
                localIndices.Add(i);
            }
        }

        if (db.patrolEnemies != null)
        {
            for (int i = 0; i < db.patrolEnemies.Count; i++)
            {
                displayNames.Add(db.patrolEnemies[i].behaviour.ToString() + " " + db.patrolEnemies[i].type.ToString());
                listIdentities.Add(true);
                localIndices.Add(i);
            }
        }

        if (displayNames.Count == 0)
        {
            EditorGUILayout.HelpBox("Оба списка врагов в базе данных пусты!", MessageType.Info);
            return;
        }

        int currentGlobalIndex = 0;
        for (int i = 0; i < displayNames.Count; i++)
        {
            if (listIdentities[i] == spawner.isPatrolList && localIndices[i] == spawner.selectedEnemyIndex)
            {
                currentGlobalIndex = i;
                break;
            }
        }

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Выбор противника из категорий базы", EditorStyles.boldLabel);

        int newGlobalIndex = EditorGUILayout.Popup("Враг для спавна:", currentGlobalIndex, displayNames.ToArray());
                
        spawner.isPatrolList = listIdentities[newGlobalIndex];
        spawner.selectedEnemyIndex = localIndices[newGlobalIndex];

        if (GUI.changed)
        {
            EditorUtility.SetDirty(spawner);
            Undo.RecordObject(spawner, "Changed Spawn Enemy Category/Index");
        }
    }
}
#endif