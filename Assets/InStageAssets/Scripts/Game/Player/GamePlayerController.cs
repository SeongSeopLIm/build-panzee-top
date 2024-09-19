

using UnityEngine;
using WAK.Game;
using WAK.Managers;

public class GamePlayerController : PlayerControllerBase
{

    public override void Initalize()
    {
        base.Initalize();
    }
     
    public void SpawnAnimalAsCursor()
    {
        // 여기에 동물 스폰

        //InputStateMachine.SwitchState(StateBase.GetOrCreate<InputState_Wait>(this));

        GameManager.Instance.SpawnAnimal(Vector2.zero);
    }
}