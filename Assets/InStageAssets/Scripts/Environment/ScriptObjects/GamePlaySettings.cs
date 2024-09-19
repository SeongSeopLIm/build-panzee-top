using System.Collections.Generic;
using UnityEngine;

namespace WAK.Game
{
    [CreateAssetMenu(fileName = "GamePlaySettings", menuName = "ScriptableObjects/GamePlaySettings")]
    public class GamePlaySettings : BaseScriptableObject<GamePlaySettings>
    {
        [SerializeField, Tooltip("CopyPath로 긁은 경로 (예: Assets/Prefabs/Actors)")]
        private string prefabFolderPath;

        [SerializeField] private List<GameObject> spawnPrefabs = new List<GameObject>();




        public string PrefabFolderPath => prefabFolderPath;
        public List<GameObject> SpawnPrefabs => spawnPrefabs;

#if UNITY_EDITOR
        public void UpdateSpawnPrefabs()
        {
            spawnPrefabs.Clear();

            // Assets 폴더를 기준으로 한 전체 경로
            string fullPath = $"{prefabFolderPath}";

            // 지정된 폴더 내의 모든 프리팹을 검색
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

            // 변경 사항을 저장
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
