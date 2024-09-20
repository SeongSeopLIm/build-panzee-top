using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCommunity.UnitySingleton;
using WAK.Game; 

namespace WAK.Managers
{

    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private GamePlaySettings gamePlaySettings; 
        public GamePlayerController PlayerController {  get; private set; } 

        public void Set(GamePlaySettings gamePlaySettings, GamePlayerController playerController)
        {
            this.gamePlaySettings = gamePlaySettings;
            this.PlayerController = playerController;
            PlayerController.Initalize();
        }

        public void Clear()
        {

        }

        /// <summary>
        /// UI를 통한 사용 금지. Stage_Play 전환으로 글로벌 상태를 변경하여 진행.
        /// </summary>
        public void Play()
        {
            PlayerController.InputStateMachine.SwitchState(StateBase.GetOrCreate<InputState_Play>(PlayerController)); 
        }

        public void Stop()
        {
            PlayerController.InputStateMachine.SwitchState(StateBase.GetOrCreate<InputState_Wait>(PlayerController)); 
        }

        public void SpawnAnimal(Vector2 screenPos)
        {
            var objectParams = new ObjectManager.ObjectParams()
            {
                dataKey = 0
            }; 

            var wakHeadImpl = ObjectManager.Instance.Spawn<WakHeadImpl>(objectParams); 
        }
         
    }
}

