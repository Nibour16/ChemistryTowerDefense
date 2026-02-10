public abstract class BaseState
{
    protected BaseStateMachine stateMachine; //Owning state machine

    // Composer
    protected BaseState(BaseStateMachine stateMachine)
    {
        // Cache this state machine owned by the state itself
        this.stateMachine = stateMachine;
    }

    protected T GetStateMachine<T>() where T : BaseStateMachine
    {
        return stateMachine as T;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
