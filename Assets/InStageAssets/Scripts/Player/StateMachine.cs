using System;

public class StateMachine<T> where T : StateBase
{
    private T currentState;

    public T CurrentState => currentState;

    public void SwitchState(T newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.Enter();
        }
    }

    public void Update()
    {
        currentState?.Update();
    }
}
