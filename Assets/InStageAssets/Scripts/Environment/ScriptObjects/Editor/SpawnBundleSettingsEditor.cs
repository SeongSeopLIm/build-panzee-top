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
        base.OnInspectorGUI();

        if (GUILayout.Button("Update Prefab List"))
        {
            SpawnBundleSettings settings = (SpawnBundleSettings)target;

            settings.UpdateSpawnPrefabs();

            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
        }

        serializedObject.ApplyModifiedProperties();

    }
}
