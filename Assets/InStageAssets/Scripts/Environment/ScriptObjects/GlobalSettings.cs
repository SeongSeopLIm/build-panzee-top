using System.Collections.Generic;
using UnityEngine;

namespace WAK.Game
{
    [CreateAssetMenu(fileName = "GlobalSettings", menuName = "ScriptableObjects/GlobalSettings")]
    public class GlobalSettings : BaseScriptableObject<GlobalSettings>
    {
        [SerializeField] private GamePlayerController defaultPlayerController;
        public GamePlayerController DefaultPlayerController => defaultPlayerController;
        [SerializeField] private GameSettings defaultGameSettings;
        public GameSettings DefaultGameSettings => defaultGameSettings;

    }
}
