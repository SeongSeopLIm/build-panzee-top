using UnityEngine;

public abstract class StateBase : IState
{
    protected PlayerControllerBase playerController;

    public StateBase(PlayerControllerBase controller)
    {
        playerController = controller;
    }

    public virtual void Enter() { }
    public virtual void Exit() { } 
    public virtual void Update() { } // 필요 없을 것 같은데 일단 추가.
}