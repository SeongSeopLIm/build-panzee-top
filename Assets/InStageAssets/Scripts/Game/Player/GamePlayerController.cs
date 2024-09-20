

using UnityEngine;
using WAK.Game;
using WAK.Managers;

public class GamePlayerController : PlayerControllerBase
{

    public override void Initalize()
    {
        base.Initalize();
        InputStateMachine.SwitchState(StateBase.GetOrCreate<InputState_Wait>(this));
        //PlayerStateMachine.SwitchState(StateBase.GetOrCreate<PlayerState_Wait>(this));
    }
     
     
    public void ReleaseCurrentHoldingObject()
    {
        GameManager.Instance.ReleaseHold();
    }
}