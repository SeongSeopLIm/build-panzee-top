
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Linq;

namespace WAK.Game
{
    [CreateAssetMenu(fileName = "SpawnBundleSettings", menuName = "ScriptableObjects/SpawnBundleSettings")]
    public class SpawnBundleSettings : BaseScriptableObject<SpawnBundleSettings>
    {
        [Serializable]
        public class SpawnBundleData
        {
            [Header("�ڵ� ������Ʈ ����")]
            public GameObject spawnPrefab;
            // ������ �� ������ ���� �ʿ�.
            public Vector2 Size;
            [Header("���� ���� ����")]
            public int probabilityCount;
        }

        [SerializeField, Tooltip("CopyPath�� ���� ��� (��: Assets/Prefabs/Actors)")]
        private string prefabFolderPath;
        [SerializeField] private List<SpawnBundleData> spawnBundleDatas = new List<SpawnBundleData>();

        // ĳ�õ� ���� Ȯ�� ����Ʈ
        private List<int> cumulativeProbabilities = new List<int>();
        private int totalCumulativeProbabilities = 0;

        public string PrefabFolderPath => prefabFolderPath;
        public List<SpawnBundleData> SpawnBundleDatas => spawnBundleDatas;

        public void UpdateCumulativeProbabilities()
        {
            cumulativeProbabilities.Clear();
            int cumulative = 0;
            foreach (var data in spawnBundleDatas)
            {
                cumulative += data.probabilityCount;
                cumulativeProbabilities.Add(cumulative);
            }
            totalCumulativeProbabilities = cumulativeProbabilities.Max();
        }

        public int GetTotalProbability()
        {
            if (cumulativeProbabilities.Count != spawnBundleDatas.Count ||
                cumulativeProbabilities.Count == 0 ||
                totalCumulativeProbabilities == 0)
            {
                UpdateCumulativeProbabilities();
            }
                 
            return totalCumulativeProbabilities;
        }

        public int GetSelectedIndex(int randomValue)
        {
            int index = cumulativeProbabilities.BinarySearch(randomValue + 1);
            if (index < 0)
            {
                index = ~index;
            }
            return index;
        }

#if UNITY_EDITOR
        public void UpdateSpawnPrefabs()
        {
            Dictionary<string, SpawnBundleData> existingPrefabsDict = new Dictionary<string, SpawnBundleData>();

            foreach (var data in spawnBundleDatas)
            {
                if (data.spawnPrefab != null)
                {
                    string existingGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(data.spawnPrefab));
                    if (!string.IsNullOrEmpty(existingGUID))
                    {
                        existingPrefabsDict[existingGUID] = data;
                    }
                }
            }

            // ������ ��ο��� ���� ��� �������� GUID�� ������
            string fullPath = prefabFolderPath;
            string[] currentPrefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { fullPath });

            List<SpawnBundleData> updatedSpawnPrefabs = new List<SpawnBundleData>();

            foreach (string guid in currentPrefabGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (prefab != null)
                {
                    prefab.TryGetComponent<SpriteRenderer>(out var spriteRenderer);
                    if (existingPrefabsDict.TryGetValue(guid, out SpawnBundleData existingData))
                    {
                        existingData.Size = spriteRenderer.bounds.size;
                        // ���� �����Ͱ� ������ �״�� ����
                        updatedSpawnPrefabs.Add(existingData);
                        existingPrefabsDict.Remove(guid);
                    }
                    else
                    {
                        SpawnBundleData newData = new SpawnBundleData()
                        {
                            spawnPrefab = prefab,
                            probabilityCount = 0,
                            Size = spriteRenderer.bounds.size,
                        };
                        updatedSpawnPrefabs.Add(newData);
                    }
                }
            }

            // ������ ������ existingPrefabsDict�� ��������. ���� ������Ʈ�� ����Ʈ���� �߰����� ����

            spawnBundleDatas = updatedSpawnPrefabs;
             
            UpdateCumulativeProbabilities();

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

            Debug.Log("SpawnPrefabs ����Ʈ�� ������Ʈ�Ǿ����ϴ�.");
        }
#endif
    }
}
