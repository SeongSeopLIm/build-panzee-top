using UnityEngine;
using UnityEngine.InputSystem;
using WAK.Managers;

namespace WAK.Game
{

    public class InputState_Play : StateBase, MainControl.IPlayActions
    {
        public InputState_Play(GamePlayerController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("InputState_Play Entered");
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("InputState_Play Exited");
        }

        public override void Update()
        {
            base.Update();

        }

        public void OnTap(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("Tap action performed ");
                if (GameManager.Instance.IsHoldingObject)
                {
                    playerController.ReleaseCurrentHoldingObject();
                }
                
            }
        } 
    }

}

