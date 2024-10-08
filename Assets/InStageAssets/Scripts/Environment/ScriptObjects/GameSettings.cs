using System.Collections.Generic;
using UnityEngine;

namespace WAK.Game
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings")]
    public class GameSettings : BaseScriptableObject<GameSettings>
    {
        [SerializeField] float spawnHeightMin;
        public float SpawnHeightMin => spawnHeightMin;
        [SerializeField] float spawnDistanceToTop;
        public float SpawnDistanceToTop => spawnDistanceToTop;

        [SerializeField] private GameObject worldPrefab;
        public GameObject WorldPrefab => worldPrefab;
        [SerializeField] private GameObject player;
        public GameObject PlayerPrefab => player;

        [SerializeField] private SpawnBundleSettings spawnBundleSettings;
        public SpawnBundleSettings SpawnBundleSettings => spawnBundleSettings;

        [SerializeField] private float spinAxisPerSec = 120f;
        public float SpinAnglePerSec => spinAxisPerSec;
    }
}
