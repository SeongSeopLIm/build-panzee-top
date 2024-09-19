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
    public virtual void Update() { } // �ʿ� ���� �� ������ �ϴ� �߰�.
}