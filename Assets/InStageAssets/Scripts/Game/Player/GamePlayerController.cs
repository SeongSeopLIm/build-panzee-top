

using WAK.Game;

public class GamePlayerController : PlayerControllerBase
{

    public override void Initalize()
    {
        base.Initalize();
    }
     
    public void SpawnAnimalAsCursor()
    {
        // ���⿡ ���� ����

        InputStateMachine.SwitchState(StateBase.GetOrCreate<InputState_Wait>(this));
    }
}