using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    #region Support Components and Modules
    private BuildStateMachine _stateMachine;
    private IGridService _gridService;

    private BuildDefinition _currentDefinition;
    private BuildPreviewHandler _previewHandler;
    #endregion

    #region Properties of Support Components and Modules
    public BuildStateMachine StateMachine => _stateMachine;
    public IGridService GridService => _gridService;
    public BuildDefinition CurrentDefinition => _currentDefinition;
    public BuildPreviewHandler PreviewHandler => _previewHandler;
    #endregion

    #region Initialization
    protected override void Awake()
    {
        base.Awake();
        Initialization();
    }
    
    private void Initialization()
    {
        _stateMachine = GetComponent<BuildStateMachine>();
        _gridService = ServiceCollector.Grid;
        _previewHandler = GetComponent<BuildPreviewHandler>();

        SecretaryBinder();
    }

    private void SecretaryBinder()
    {
        _previewHandler.BindManager(this);
    }
    #endregion

    #region Public API
    public void OnTowerSelected(GameObject prefab)
    {
        if (_currentDefinition != null)
        {
            ClearDefinition();
            return;
        }
        
        if (prefab == null) return; // If prefab is empty, do nothing

        var definition = new BuildDefinition(prefab);   // Create new definition

        // If prefab does not contain tower stat, do not change to build state
        if (definition.TowerStat == null)   return;

        _currentDefinition = definition;    // Set the current definition

        // Request to enter build state
        _stateMachine.SetState(_stateMachine.BuildState);
    }

    public void ClearDefinition()
    {
        _stateMachine.SetState(_stateMachine.NormalState);
        _currentDefinition = null;
    }

    public void OnBuildFinished()
    {

    }
    #endregion
}
