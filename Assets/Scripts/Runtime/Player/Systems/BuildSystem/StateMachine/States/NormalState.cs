using UnityEngine;

public class NormalState : BaseState
{
    private BuildStateMachine _stateMachine;

    #region Initialization
    public NormalState(BaseStateMachine  stateMachine) : base(stateMachine) 
    {
        ResolveDependencies();
    }

    private void ResolveDependencies()
    {
        _stateMachine = GetStateMachine<BuildStateMachine>();
        if (_stateMachine == null)
        {
            Debug.LogError($"{nameof(BuildStateMachine)} requires {nameof(BuildStateMachine)}");
        }
    }
    #endregion

    #region State Lifecycle
    public override void EnterState() 
    {
        // TODO: write logic when entering this state here
    }
    public override void UpdateState() 
    {
        // TODO: write logic during this state update here;
    }
    public override void ExitState() 
    {
        // TODO: write logic when exiting this state here
    }
    #endregion
}