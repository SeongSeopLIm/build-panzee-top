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
            GameManager.Instance.MainControls.play.SetCallbacks(this);
        }

        public override void Exit()
        {
            GameManager.Instance.MainControls.play.RemoveCallbacks(this);
            Debug.Log("InputState_Play Exited");
            base.Exit();
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

