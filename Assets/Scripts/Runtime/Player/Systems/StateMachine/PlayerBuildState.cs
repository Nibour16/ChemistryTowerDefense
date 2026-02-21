using UnityEngine;

public class PlayerBuildState : BaseState
{
    private PlayerStateMachine _stateMachine;

    #region Initialization
    public PlayerBuildState(BaseStateMachine  stateMachine) : base(stateMachine) 
    {
        ResolveDependencies();
    }

    private void ResolveDependencies()
    {
        _stateMachine = GetStateMachine<PlayerStateMachine>();
        if (_stateMachine == null)
        {
            Debug.LogError($"{nameof(PlayerBuildState)} requires {nameof(PlayerStateMachine)}");
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
        // TODO: write logic during this state update here
    }
    public override void ExitState() 
    {
        // TODO: write logic when exiting this state here
    }
    #endregion
}