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
    public GridBlockerCollector BlockerCollector  => blockerCollector;
    public IReadOnlyList<Bounds> BlockingBounds => _blockingBounds;
    #endregion

    #region Events
    public event Action<int, int> OnCellDataUpdated;
    #endregion

    #region Initialization
    protected override void Awake()
    {
        base.Awake();
        
        InitializeSecretaries();
        InitializeGridData();
        InitializeRuntimeModules();
    }

    private void InitializeSecretaries()
    {
        _secretaryBinder = new GridSecretaryBinder(this);
        _secretaryBinder.Bind();

        _gridGenerator = _secretaryBinder.Generator;
        _gridDetector = _secretaryBinder.Detector;
        blockerCollector = _secretaryBinder.BlockerCollector;
    }

    private void InitializeGridData()
    {
        _dataInitializer = new GridDataInitializer(_gridGenerator, blockerCollector);
        _dataInitializer.Initialize();

        _stateDataBase = _dataInitializer.StateDataBase;
        _blockingBounds = _dataInitializer.BlockingBounds;
    }

    private void InitializeRuntimeModules()
    {
        // System Coordination
        _coordinate = new GridCoordinateSystem(_gridGenerator, _stateDataBase);

        // State Synchronizer
        _stateSync = new GridStateSynchronizer(_gridDetector, _stateDataBase, NotifyCellUpdated);
        _stateSync.ApplyInitialState();

        // Grid Occupation Handler
        _occupation = new GridOccupationHandler(_stateDataBase, NotifyCellUpdated);
    }
    #endregion

    #region Public API
    public Vector3 GetCellCenter(int x, int z)
        => _coordinate.GetCellCenter(x, z);

    public bool WorldToCell(Vector3 worldPos, out int x, out int z)
        => _coordinate.WorldToCell(worldPos, out x, out z);

    public bool IsInsideGrid(int x, int z)
        => _coordinate.IsInsideGrid(x, z);

    public bool IsCellBlocked(int x, int z)
        => _stateSync.IsCellBlocked(x, z);

    public bool TryOccupy(int x, int z, BaseTower tower)
        => _occupation.TryOccupy(x, z, tower);

    public void ClearOccupation(int x, int z)
        => _occupation.ClearOccupation(x, z);
    #endregion

    #region Internal Callback
    private void NotifyCellUpdated(int x, int z)
    {
        OnCellDataUpdated?.Invoke(x, z);
    }
    #endregion
}
