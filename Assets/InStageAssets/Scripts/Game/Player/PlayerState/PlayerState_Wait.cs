// PlayerState.cs
using UnityEngine;

namespace WAK.Game
{

    public class PlayerState_Wait : StateBase
    {
        public PlayerState_Wait(PlayerControllerBase controller) : base(controller)
        {
        }

        public override void Enter()
        {
            Debug.Log("PlayerState Entered");

        }

        public override void Exit()
        {
            Debug.Log("PlayerState Exited");

        }

        public override void Update()
        {

        }
          
    }

}
