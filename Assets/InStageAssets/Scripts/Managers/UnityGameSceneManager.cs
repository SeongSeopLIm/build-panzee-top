using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCommunity.UnitySingleton;
using Cysharp.Threading.Tasks;

namespace WAK.Managers
{ 
    /// <summary>
    /// 유니티 씬 전환만 관리. 게임 글로벌 스테이트 관리는 StageManager 사용.
    /// </summary>
    public class UnityGameSceneManager : PersistentMonoSingleton<UnityGameSceneManager>
    {
        public enum SceneType
        {
            None = -1, // not setted
            scene_initial,
            scene_lobby_and_game // 로비와 게임 플레이 같은 씬에서 진행
        }

        public SceneType CurrentScene { get; private set; } = SceneType.None;

        public async UniTask LoadSceneAsync(SceneType sceneType)
        {
            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneType.ToString());
            CurrentScene = sceneType;
        }
    }
}

