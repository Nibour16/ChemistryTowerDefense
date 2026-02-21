using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private PlayerStateMachine _stateMachine;
    private BuildManager _buildManager;
    public PlayerStateMachine StateMachine => _stateMachine;
    public BuildManager BuildManager => _buildManager;

    #region Initialization
    protected override void Awake()
    {
        base.Awake();

        StateMachineNullCheck();
        InitializeBuildManager();
        OnInitializationComplete();
    }

    private void StateMachineNullCheck()
    {
        if (_stateMachine == null)
        {
            Debug.Log("Player State Machine is not assigned, attempt to find it in the object");
            _stateMachine = GetComponent<PlayerStateMachine>();

            if (_stateMachine == null)
                Debug.LogError("Not found!");
            else
            {
                #if UNITY_EDITOR
                Debug.Log("Find successfully!");
                #endif
            }
        }
    }

    private void InitializeBuildManager()
    {
        _buildManager = BuildManager.Instance;
        _buildManager.OnBuildRequested += HandleBuildRequested;
    }

    private void OnInitializationComplete()
    {
        _stateMachine.BuildState.OnBuildExited += HandleBuildExited;
    }
    #endregion

    #region Build System Support
    private void HandleBuildRequested(BuildDefinition definition)
    {
        _stateMachine.SetState(_stateMachine.BuildState);
    }

    private void HandleBuildExited()
    {
        _buildManager.ClearDefinition();
    }

    private void OnDestroy()
    {
        _buildManager.OnBuildRequested -= HandleBuildRequested;
        _stateMachine.BuildState.OnBuildExited -= HandleBuildExited;
    }
    #endregion
}
