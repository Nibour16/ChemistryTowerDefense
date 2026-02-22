using UnityEngine;

public class BuildState : BaseState
{
    private BuildStateMachine _stateMachine;
    private IWorldPointerService _worldPointer;
    private InputManager _inputManager;

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
        _inputManager = InputManager.Instance;
        _worldPointer = ServiceLocator.GetRequired<IWorldPointerService>();

        _stateMachine.BuildManager.PreviewHandler.CreateGhost();
    }

    public override void UpdateState() 
    {
        var screenPos = _inputManager.PointPosition();

        if (_worldPointer.TryGetWorldPosition(screenPos, out Vector3 previewPosition))
            _stateMachine.BuildManager.PreviewHandler.UpdateGhostPosition(previewPosition);
    }

    public override void ExitState() 
    {
        _inputManager = null;
        _worldPointer = null;
    }
    #endregion
}