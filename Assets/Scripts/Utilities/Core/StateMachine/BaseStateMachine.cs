using UnityEngine;

public abstract class BaseStateMachine : MonoBehaviour
{
    protected BaseState currentState;
    public BaseState CurrentState => currentState;

    public virtual void SetState(BaseState newState)
    {
        // exit the current state if it is available
        currentState?.ExitState();
        // Set the new state
        currentState = newState;
        // Enter the new state
        currentState.EnterState();
    }

    public virtual void Update()
    {
        currentState?.UpdateState();
    }
}
