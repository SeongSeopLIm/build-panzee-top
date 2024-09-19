using UnityEngine;
using UnityEngine.InputSystem;

namespace WAK.Game
{

    public class InputState_Play : StateBase, MainControl.IPlayActions
    {
        public InputState_Play(PlayerControllerBase controller) : base(controller)
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
                //playerController.�����ƶ�������;
            }
        } 
    }

}
