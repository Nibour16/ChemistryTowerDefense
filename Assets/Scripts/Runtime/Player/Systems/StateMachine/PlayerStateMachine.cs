using UnityEngine;

public class PlayerStateMachine : BaseStateMachine
{
    #region Keep track of all states
    private PlayerNormalState _normalState;
    private PlayerBuildState _buildState;
    #endregion

    #region Referencing all of states
    public PlayerNormalState NormalState => _normalState;
    public PlayerBuildState BuildState => _buildState;
    #endregion

    #region Keep track of all supporting components
    private PlayerManager _manager;
    #endregion

    #region Referencing all supporting components
    public PlayerManager Manager => _manager;
    #endregion

    private void Awake()
    {
        _normalState = new PlayerNormalState(this);
        _buildState = new PlayerBuildState(this);

        _manager = PlayerManager.Instance;
    }

    private void Start()
    {
        SetState(_normalState);
    }
}