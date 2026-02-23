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
        UpdateGhost();
    }

    public override void ExitState() 
    {
        
    }
    #endregion

    #region Build Preview Application
    private void UpdateGhost()
    {
        var screenPos = _inputManager.PointPosition();

        if (!Application.isFocused || !IsMouseInsideScreen(screenPos))
        {
            _stateMachine.BuildManager.PreviewHandler.HideGhost();
            return;
        }

        _stateMachine.BuildManager.PreviewHandler.ShowGhost();

        if (_worldPointer.TryGetProjectedPosition(screenPos, out Vector3 previewPosition))
            _stateMachine.BuildManager.PreviewHandler.UpdateGhostPosition(previewPosition);
    }

    private bool IsMouseInsideScreen(Vector3 m)
    {
        return m.x >= 0 && m.x <= Screen.width &&
               m.y >= 0 && m.y <= Screen.height;
    }
    #endregion
}