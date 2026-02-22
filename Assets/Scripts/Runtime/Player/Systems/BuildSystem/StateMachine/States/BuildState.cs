using System;
using UnityEngine;

public class BuildState : BaseState
{
    private BuildStateMachine _stateMachine;

    #region Initialization
    public BuildState(BaseStateMachine  stateMachine) : base(stateMachine) 
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
        
    }
    public override void UpdateState() 
    {
        
    }
    public override void ExitState() 
    {
        
    }
    #endregion
}