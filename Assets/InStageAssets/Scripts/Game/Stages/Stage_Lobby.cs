using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WAK.Core;
using WAK.Managers;

namespace WAK.Game
{
    public class Stage_Lobby : StageBase
    {
        public override void Enter()
        {
            base.Enter();
            if(UnityGameSceneManager.Instance.CurrentScene == UnityGameSceneManager.SceneType.scene_lobby_and_game)
            {
                var globalSettings = Framework.Instance.GlobalSettings;
                GameManager.Instance.Set(gameSettings: globalSettings.DefaultGameSettings,
                    playerController: globalSettings.DefaultPlayerController);
            }
            else
            {
                //enter from splash stage
                LoadLobbySceneAndSetGameAsync().Forget();
            } 
        }

        public override void Exit()
        {
            base.Exit();
        }

        private async UniTaskVoid LoadLobbySceneAndSetGameAsync()
        {
            await UnityGameSceneManager.Instance.LoadSceneAsync(UnityGameSceneManager.SceneType.scene_lobby_and_game);

            var globalSettings = Framework.Instance.GlobalSettings;
            GameManager.Instance.Set(gameSettings: globalSettings.DefaultGameSettings,
                playerController: globalSettings.DefaultPlayerController);
        }
    }
}
