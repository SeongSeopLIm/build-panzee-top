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
        /// UI�� ���� ��� ����. Stage_Play ��ȯ���� �۷ι� ���¸� �����Ͽ� ����.
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

