

using WAK.Game;

public class GamePlayerController : PlayerControllerBase
{

    public override void Initalize()
    {
        base.Initalize();
    }
     
    public void SpawnAnimalAsCursor()
    {
        // 여기에 동물 스폰

        InputStateMachine.SwitchState(StateBase.GetOrCreate<InputState_Wait>(this));
    }
}