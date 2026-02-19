using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridGenerator3D))]
[RequireComponent(typeof(GridDetector))]
public class GridManager : Singleton<GridManager>
{
    // Secretary class components of grid system
    [SerializeField] private GridBlockerCollector blockerCollector; // Blocker collector reference
    private GridGenerator3D _gridGenerator; // Grid generator reference
    private GridDetector _gridDetector; // Grid detector reference

    // Properties of secretary classes
    public GridGenerator3D GridGenerator => _gridGenerator;
    //public GridBlockerCollector BlockerCollector => blockerCollector;
    //public GridDetector GridDetector => _gridDetector;

    // Grid system data collectors
    private GridStateDataBase _stateDataBase;
    private IReadOnlyList<Bounds> _blockingBounds;

    // Grid system data properties
    public GridStateDataBase StateDataBase => _stateDataBase;
    public IReadOnlyList<Bounds> BlockingBounds => _blockingBounds;

    // Subject during data update
    public event Action<int, int> OnCellDataUpdated;

    #region Initialization
    protected override void Awake()
    {
        base.Awake();

        SecretaryInitialization();
        GridInitialization();
    }

    private void SecretaryInitialization()
    {
        // Assign the secretary components that allows system members to apply
        _gridGenerator = GetComponent<GridGenerator3D>();
        _gridDetector = GetComponent<GridDetector>();

        if (blockerCollector == null)
        {
            blockerCollector = GetComponent<GridBlockerCollector>();

            if (blockerCollector == null)
            {
                Debug.LogWarning(
                    "GridManager: No GridBlockerCollector found. Grid will assume no blocking objects.",
                    this
                );
            }
        }

        // Assign all grid secretary components about their manager
        blockerCollector.BindManager(this);
        _gridGenerator.BindManager(this);
        _gridDetector.BindManager(this);
    }

    private void GridInitialization()
    {
        // Initialize tower grid
        InitializeTowerGrid();

        if (blockerCollector != null)
        {
            blockerCollector.Collect();
            SetBlockingBounds(blockerCollector.BlockingBounds);
        }

        ApplyStatesToGrid();
    }

    private void InitializeTowerGrid()
    {
        int w = _gridGenerator.GridWidth;
        int h = _gridGenerator.GridHeight;

        _stateDataBase = new GridStateDataBase(w, h);
    }

    private void SetBlockingBounds(IReadOnlyList<Bounds> bounds)
    {
        _blockingBounds = bounds;
    }
    #endregion

    #region State Update
    private void ApplyStatesToGrid()
    {
        // Summary the detection of the detector
        var blockedMap = _gridDetector.DetectAllCells();

        // No data or detection is not done, do nothing
        if (_stateDataBase == null || blockedMap == null)
            return;

        // Go over the cells to update the state
        int w = _stateDataBase.Width;
        int h = _stateDataBase.Height;

        for (int x = 0; x < w; x++)
        {
            for (int z = 0; z < h; z++)
                ApplyStateToCell(x, z, blockedMap[x, z]);
        }
    }

    public void ApplyStateToCell(int x, int z, bool blocked)
    {
        if (!IsInsideGrid(x, z))
            return;

        GridCellState oldState = _stateDataBase.GetState(x, z);

        // Prevent map update that will mess up the tower occupation
        /*if (ShouldPreventOverride(oldState))
            return;*/

        GridCellState newState =
            blocked ? GridCellState.NotPlaceable : GridCellState.Empty;

        // If state is not changed, do nothing
        if (oldState == newState)
            return;

        UpdateData(newState, x, z);
    }

    /*private bool ShouldPreventOverride(GridCellState oldState)
    {
        return (oldState == GridCellState.TowerOccupied);
    }*/
    #endregion

    // TODO: Could but recommended to be internal when all systems are stabilized
    // Data Update Core Handler
    public void UpdateData(GridCellState newState, int x, int z)
    {
        _stateDataBase.SetState(x, z, newState);
        OnCellDataUpdated?.Invoke(x, z);
    }

    #region Grid System public methods
    // Get world position of the center of the grid cell
    public Vector3 GetCellCenter(int x, int z)
    {
        return new Vector3(
            _gridGenerator.Origin.x + x * _gridGenerator.CellSize,
            _gridGenerator.FixedY,
            _gridGenerator.Origin.z + z * _gridGenerator.CellSize
        );
    }

    // Convert world position to grid cell position
    public bool WorldToCell(Vector3 worldPos, out int x, out int z)
    {
        Vector3 localPos = worldPos - _gridGenerator.Origin;
        x = Mathf.FloorToInt(localPos.x / _gridGenerator.CellSize);
        z = Mathf.FloorToInt(localPos.z / _gridGenerator.CellSize);
        return x >= 0 && x < _gridGenerator.GridWidth && z >= 0 && z < _gridGenerator.GridHeight;
    }

    public bool IsCellBlocked(int x, int z)
    {
        return _gridDetector != null && _gridDetector.IsCellBlocked(x, z);
    }

    public bool IsInsideGrid(int x, int z)
    {
        return _stateDataBase.IsInside(x, z);
    }
    #endregion
}
