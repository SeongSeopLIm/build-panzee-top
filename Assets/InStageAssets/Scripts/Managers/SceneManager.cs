using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCommunity.UnitySingleton;
using Cysharp.Threading.Tasks;

namespace WAK.Managers
{ 
    public class SceneManager : PersistentMonoSingleton<SceneManager>
    {
        public enum SceneType
        {
            scene_initial,
            scene_lobby_and_game // 로비와 게임 플레이 같은 씬에서 진행
        }

        
        
        public async UniTask LoadSceneAsync(SceneType sceneType)
        {
            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneType.ToString());

        }
    }
}

