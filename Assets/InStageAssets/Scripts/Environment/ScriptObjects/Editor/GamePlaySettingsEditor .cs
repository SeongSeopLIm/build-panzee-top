using UnityEngine;
using UnityEditor;
using WAK.Game;

[CustomEditor(typeof(SpawnBundleSettings))]
public class SpawnBundleSettingsEditor : Editor
{
    SerializedProperty prefabFolderPathProp;
    SerializedProperty spawnPrefabsProp;

    private void OnEnable()
    {
        prefabFolderPathProp = serializedObject.FindProperty("prefabFolderPath");
        spawnPrefabsProp = serializedObject.FindProperty("spawnPrefabs");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(prefabFolderPathProp, new GUIContent("Prefab Folder Path", "Assets 폴더를 기준으로 한 상대 경로를 입력하세요 (예: Prefabs/Actors)"));

        if (GUILayout.Button("Update Prefab List"))
        {
            SpawnBundleSettings settings = (SpawnBundleSettings)target;

            settings.UpdateSpawnPrefabs();

            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
        }

        EditorGUILayout.PropertyField(spawnPrefabsProp, new GUIContent("Spawn Prefabs"), true);

        serializedObject.ApplyModifiedProperties();
    }
}
