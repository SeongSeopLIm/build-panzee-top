
using System.Collections.Generic;
using UnityEngine;

namespace WAK.Game
{

    [CreateAssetMenu(fileName = "GamePlaySettings", menuName = "ScriptableObjects/GamePlaySettings")]
    public class GamePlaySettings : BaseScriptableObject<GamePlaySettings>
    {
        [SerializeField] private List<GameObject> spawnSprites;


    }
}
