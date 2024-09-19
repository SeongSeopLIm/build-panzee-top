using UnityEngine;
using UnityEngine.InputSystem;

namespace WAK.Game
{

    public class InputState_Wait : StateBase, MainControl.IPlayActions
    {
        public InputState_Wait(PlayerControllerBase controller) : base(controller)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("InputState_Wait Entered");
        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("InputState_Wait Exited");
        }

        public override void Update()
        {
            base.Update();

        }

        public void OnTap(InputAction.CallbackContext context)
        {
            // 요기 상태에서는 스킵
            if (context.performed)
            {
                Debug.Log("InputState_Wait Tap action performed "); 
            }
        } 
    }

}

