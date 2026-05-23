#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyDatabase))]
public class EnemyDatabaseEditor : Editor
{
    private Type[] _movementTypes;
    private Type[] _attackingTypes;

    private void OnEnable()
    {
        _movementTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IMovementStrategy).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .ToArray();

        _attackingTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IAttackingStrategy).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .ToArray();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty enemiesProp = serializedObject.FindProperty("enemies");
        EditorGUILayout.LabelField("Сконструированные враги", EditorStyles.boldLabel);

        for (int i = 0; i < enemiesProp.arraySize; i++)
        {
            SerializedProperty element = enemiesProp.GetArrayElementAtIndex(i);
            SerializedProperty nameProp = element.FindPropertyRelative("enemyName");
            SerializedProperty prefabProp = element.FindPropertyRelative("prefab");
            SerializedProperty movingStrategyProp = element.FindPropertyRelative("movementStrategy");
            SerializedProperty attackingStrategyProp = element.FindPropertyRelative("attackingStrategy");

            bool hasPrefabError = prefabProp.objectReferenceValue == null;
            bool hasMoveError = movingStrategyProp.managedReferenceValue == null;
            bool hasAttackError = attackingStrategyProp.managedReferenceValue == null;
            bool hasAnyError = hasPrefabError || hasMoveError || hasAttackError;

            Color originalColor = GUI.backgroundColor;
            
            if (hasAnyError) 
                GUI.backgroundColor = new Color(1f, 0.75f, 0.75f);

            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(nameProp, new GUIContent("Enemy Name"));
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space(5);

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(prefabProp, new GUIContent($"Prefab"));

            if (EditorGUI.EndChangeCheck())
            {
                UpdateEnemyName(nameProp, prefabProp, movingStrategyProp);
            }

            EditorGUILayout.Space(5);

            DrawStrategyPopup("Movement:", movingStrategyProp, _movementTypes);
            DrawStrategyPopup("Attack:", attackingStrategyProp, _attackingTypes);

            if (hasAnyError)
            {
                EditorGUILayout.Space(5);
                if (hasPrefabError)
                {
                    EditorGUILayout.HelpBox("Ошибка: Обязательно перетащите префаб врага!", MessageType.Error);
                }
                if (hasMoveError)
                {
                    EditorGUILayout.HelpBox("Ошибка: Не выбрана стратегия движения.", MessageType.Warning);
                }
                if (hasAttackError)
                {
                    EditorGUILayout.HelpBox("Ошибка: Не выбрана стратегия атаки.", MessageType.Info);
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                UpdateEnemyName(nameProp, prefabProp, movingStrategyProp);
            }

            if (GUILayout.Button("Удалить врага", GUILayout.Width(100)))
            {
                enemiesProp.DeleteArrayElementAtIndex(i);
                break;
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }

        if (GUILayout.Button("Добавить нового врага"))
        {
            enemiesProp.arraySize++;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawStrategyPopup(string label, SerializedProperty strategyProp, Type[] types)
    {
        string currentTypeName = strategyProp.managedReferenceValue != null
            ? strategyProp.managedReferenceValue.GetType().Name
            : "None";

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, EditorStyles.miniLabel, GUILayout.Width(130));

        if (GUILayout.Button(currentTypeName, EditorStyles.popup))
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("None"), strategyProp.managedReferenceValue == null, () => {
                strategyProp.managedReferenceValue = null;
                strategyProp.serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(strategyProp.serializedObject.targetObject);
            });

            foreach (var type in types)
            {
                bool isCurrent = strategyProp.managedReferenceValue != null && strategyProp.managedReferenceValue.GetType() == type;
                menu.AddItem(new GUIContent(type.Name), isCurrent, () => {
                    strategyProp.managedReferenceValue = Activator.CreateInstance(type);
                    strategyProp.serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(strategyProp.serializedObject.targetObject);
                });
            }
            menu.ShowAsContext();
        }
        EditorGUILayout.EndHorizontal();

        if (strategyProp.managedReferenceValue != null)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(strategyProp, new GUIContent("Параметры"), true);
            EditorGUI.indentLevel--;
        }
    }

    private void UpdateEnemyName(SerializedProperty nameProp, SerializedProperty prefabProp, SerializedProperty moveProp)
    {
        if (prefabProp.objectReferenceValue == null)
        {
            nameProp.stringValue = "";
            return;
        }

        string prefabName = prefabProp.objectReferenceValue.name;
        string moveName = moveProp.managedReferenceValue != null
            ? moveProp.managedReferenceValue.GetType().Name
            : "NoMovement";

        nameProp.stringValue = $"{prefabName} {moveName}";
    }
}
#endif