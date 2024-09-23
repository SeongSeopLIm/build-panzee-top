#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace WAK.Game
{
    [CreateAssetMenu(fileName = "SpawnBundleSettings", menuName = "ScriptableObjects/SpawnBundleSettings")]
    public class SpawnBundleSettings : BaseScriptableObject<SpawnBundleSettings>
    {
        [Serializable]
        public class SpawnBundleData
        {
            public GameObject spawnPrefab;
            public int probabilityCount;
        }

        [SerializeField, Tooltip("CopyPath로 긁은 경로 (예: Assets/Prefabs/Actors)")]
        private string prefabFolderPath;
        [SerializeField] private List<SpawnBundleData> spawnBundleDatas = new List<SpawnBundleData>();

        // 캐시된 누적 확률 리스트
        private List<int> cumulativeProbabilities = new List<int>();

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
        }

        public int GetTotalProbability()
        {
            if (cumulativeProbabilities.Count > 0)
                return cumulativeProbabilities[cumulativeProbabilities.Count - 1];
            return 0;
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

            // 지정된 경로에서 현재 모든 프리팹의 GUID를 가져옴
            string fullPath = prefabFolderPath;
            string[] currentPrefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { fullPath });

            List<SpawnBundleData> updatedSpawnPrefabs = new List<SpawnBundleData>();

            foreach (string guid in currentPrefabGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (prefab != null)
                {
                    if (existingPrefabsDict.TryGetValue(guid, out SpawnBundleData existingData))
                    {
                        // 기존 데이터가 있으면 그대로 유지
                        updatedSpawnPrefabs.Add(existingData);
                        existingPrefabsDict.Remove(guid);
                    }
                    else
                    {
                        SpawnBundleData newData = new SpawnBundleData()
                        {
                            spawnPrefab = prefab,
                            probabilityCount = 0
                        };
                        updatedSpawnPrefabs.Add(newData);
                    }
                }
            }

            // 삭제된 에셋은 existingPrefabsDict에 남아있음. 따라서 업데이트된 리스트에는 추가하지 않음

            spawnBundleDatas = updatedSpawnPrefabs;

            // 누적 확률 업데이트
            UpdateCumulativeProbabilities();

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

            Debug.Log("SpawnPrefabs 리스트가 업데이트되었습니다.");
        }
#endif
    }
}
#endif
