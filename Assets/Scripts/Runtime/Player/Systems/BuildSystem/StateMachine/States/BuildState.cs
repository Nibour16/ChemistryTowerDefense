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

    #region Build System Application
    private void UpdateGhost() 
    { 
        var screenPos = _inputManager.PointPosition(); 
        if (!Application.isFocused || !IsMouseInsideScreen(screenPos)) 
        { 
            _stateMachine.BuildManager.PreviewHandler.HideGhost(); return; 
        } 
        _stateMachine.BuildManager.PreviewHandler.ShowGhost(); 
        if (_worldPointer.TryGetProjectedPosition(screenPos, out Vector3 previewPos)) 
        { 
            _stateMachine.BuildManager.PreviewHandler.ApplyGhostVisualPosition
                (previewPos, out var finalPos);

            HandlePlacementConfirm(finalPos); 
        } 
    }

    private bool IsMouseInsideScreen(Vector3 m)
    {
        return m.x >= 0 && m.x <= Screen.width &&
               m.y >= 0 && m.y <= Screen.height;
    }

    /*private Vector3 GetWorldPosition(Vector3 screenPos, out bool isValid)
    {
        isValid = true;

        if (_worldPointer.TryGetWorldPosition(screenPos, out var hitPos))
        {
            return hitPos;
        }
            
        else if (_worldPointer.TryGetProjectedPosition(screenPos, out var projected))
        {
            return projected;
        }

        isValid = false;
        return default;
    }*/

    private void HandlePlacementConfirm(Vector3 worldPos)
    {
        if (_inputManager.IsSelected())
        {
            var placementHandler = _stateMachine.BuildManager.PlacementHandler;

            bool success = placementHandler.TryPromoteGhost(
                worldPos, _stateMachine.BuildManager.CurrentDefinition,
                _stateMachine.BuildManager.PreviewHandler);

            if (success)
                _stateMachine.BuildManager.OnBuildFinished();
        }
    }
    #endregion
}