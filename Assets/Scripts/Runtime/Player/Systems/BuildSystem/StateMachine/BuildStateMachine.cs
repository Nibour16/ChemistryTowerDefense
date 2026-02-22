using UnityEngine;

public class BuildStateMachine : BaseStateMachine
{
    #region Keep track of all states
    private NormalState _normalState;
    private BuildState _buildState;
    #endregion

    #region Referencing all of states
    public NormalState NormalState => _normalState;
    public BuildState BuildState => _buildState;
    #endregion

    #region Keep track of all supporting components

    #endregion

    #region Referencing all supporting components

    #endregion

    public override void SetState(BaseState newState)
    {
        if (currentState == newState) return;

        base.SetState(newState);
    }

    private void Awake()
    {
        _normalState = new NormalState(this);
        _buildState = new BuildState(this);
    }

    private void Start()
    {
        SetState(_normalState);
    }
}