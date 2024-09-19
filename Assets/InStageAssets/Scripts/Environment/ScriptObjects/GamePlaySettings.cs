using System.Collections.Generic;
using UnityEngine;

namespace WAK.Game
{
    [CreateAssetMenu(fileName = "GamePlaySettings", menuName = "ScriptableObjects/GamePlaySettings")]
    public class GamePlaySettings : BaseScriptableObject<GamePlaySettings>
    {
        [SerializeField, Tooltip("CopyPath�� ���� ��� (��: Assets/Prefabs/Actors)")]
        private string prefabFolderPath;

        [SerializeField] private List<GameObject> spawnPrefabs = new List<GameObject>();




        public string PrefabFolderPath => prefabFolderPath;
        public List<GameObject> SpawnPrefabs => spawnPrefabs;

#if UNITY_EDITOR
        public void UpdateSpawnPrefabs()
        {
            spawnPrefabs.Clear();

            // Assets ������ �������� �� ��ü ���
            string fullPath = $"{prefabFolderPath}";

            // ������ ���� ���� ��� �������� �˻�
            string[] prefabGuids = UnityEditor.AssetDatabase.FindAssets("t:Prefab", new[] { fullPath });
            foreach (string guid in prefabGuids)
            {
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (prefab != null)
                {
                    spawnPrefabs.Add(prefab);
                }
            }

            // ���� ������ ����
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}