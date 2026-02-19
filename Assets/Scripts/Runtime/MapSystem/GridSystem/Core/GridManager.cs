using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridGenerator3D))]
[RequireComponent(typeof(GridDetector))]
public class GridManager : Singleton<GridManager>
{
    #region Secretary Components
    [SerializeField] private GridBlockerCollector blockerCollector;

    private GridGenerator3D _gridGenerator;
    private GridDetector _gridDetector;
    private GridFacade _gridFacade;
    #endregion

    #region Core Data
    private GridStateDataBase _stateDataBase;
    private IReadOnlyList<Bounds> _blockingBounds;
    #endregion

    #region Modules
    private GridSecretaryBinder _secretaryBinder;
    private GridDataInitializer _dataInitializer;
    private GridCoordinateSystem _coordinate;
    private GridStateSynchronizer _stateSync;
    private GridOccupationHandler _occupation;
    #endregion

    #region Properties
    public GridGenerator3D GridGenerator => _gridGenerator;
    public GridStateDataBase StateDataBase => _stateDataBase;
    public IReadOnlyList<Bounds> BlockingBounds => _blockingBounds;
    #endregion

    #region Events
    public event Action<int, int> OnCellDataUpdated;
    #endregion

    #region Initialization
    protected override void Awake()
    {
        base.Awake();

        InitializeModules();
        InitializeSecretaries();
        InitializeGridData();
    }

    private void InitializeSecretaries()
    {
        _secretaryBinder.Bind();

        _gridGenerator = _secretaryBinder.Generator;
        _gridDetector = _secretaryBinder.Detector;
        blockerCollector = _secretaryBinder.BlockerCollector;
    }

    private void InitializeGridData()
    {
        _dataInitializer.Initialize();

        _stateDataBase = _dataInitializer.StateDataBase;
        _blockingBounds = _dataInitializer.BlockingBounds;
    }

    private void InitializeModules()
    {
        // Initializer Modules
        _secretaryBinder = new GridSecretaryBinder(this);
        _dataInitializer = new GridDataInitializer(_gridGenerator, blockerCollector);

        // new GridCoordinateSystem
        // new GridOccupationHandler
        // new GridStateSynchronizer
    }
    #endregion

    #region Public API
    public Vector3 GetCellCenter(int x, int z) => Vector3.zero;
        //=> _coordinate.GetCellCenter(x, z);

    public bool WorldToCell(Vector3 worldPos, out int x, out int z) { x = 0; z = 0; return false; }
        //=> _coordinate.WorldToCell(worldPos, out x, out z);

    public bool IsCellBlocked(int x, int z) => false;
        //=> _stateSync.IsCellBlocked(x, z);

    public bool IsInsideGrid(int x, int z) => false;
        //=> _coordinate.IsInsideGrid(x, z);

    public bool TryOccupy(int x, int z, BaseTower tower) => false;
        //=> _occupation.TryOccupy(x, z, tower);

    public void ClearOccupation(int x, int z) { }
        //=> _occupation.ClearOccupation(x, z);
    #endregion

    #region Internal Callback
    private void NotifyCellUpdated(int x, int z)
    {
        OnCellDataUpdated?.Invoke(x, z);
    }
    #endregion
}
