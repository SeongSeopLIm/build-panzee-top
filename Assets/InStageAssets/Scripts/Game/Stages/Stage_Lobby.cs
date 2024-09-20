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
            var globalSettings = Framework.Instance.GlobalSettings;
            GameManager.Instance.Set(gamePlaySettings: globalSettings.DefaultPlaySettings,
                playerController: globalSettings.DefaultPlayerController );
            UnityGameSceneManager.Instance.LoadSceneAsync(UnityGameSceneManager.SceneType.scene_lobby_and_game).Forget();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
