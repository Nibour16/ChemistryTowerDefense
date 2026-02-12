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
    private GridCellData[,] _gridData;
    private IReadOnlyList<Bounds> _blockingBounds;

    // Grid system data properties
    public GridCellData[,] GridData => _gridData;
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
        _gridData = new GridCellData[w, h];
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
        if (_gridData == null || blockedMap == null)
            return;

        // Go over the cells to updatethe state
        int w = _gridData.GetLength(0);
        int h = _gridData.GetLength(1);

        for (int x = 0; x < w; x++)
        {
            for (int z = 0; z < h; z++)
                ApplyStateToCell(x, z, blockedMap[x, z]);
        }
    }

    public void ApplyStateToCell(int x, int z, bool blocked)
    {
        GridCellData oldData = _gridData[x, z];

        GridCellState newState =
            blocked ? GridCellState.NotPlaceable : GridCellState.Empty;

        // If state is not changed, do nothing
        if (oldData != null && oldData.state == newState)
            return;

        BaseTower tower = oldData?.tower;

        GridCellData newData = new GridCellData
        {
            tower = tower,   // Keep the old tower data
            state = newState    // As we are just updating the state
        };

        UpdateData(newData, x, z);  // Assign the new data to the desired place
    }
    #endregion

    // TODO: Could but recommended to be internal when all systems are stabilized
    // Data Update Core Handler
    public void UpdateData(GridCellData newData, int row, int column)
    {
        if (_gridData == null)
            return;

        if (row < 0 || row >= _gridGenerator.GridWidth ||
            column < 0 || column >= _gridGenerator.GridHeight)
            return;

        if (_gridData[row, column] == null)
            _gridData[row, column] = new GridCellData();

        _gridData[row, column].tower = newData.tower;
        _gridData[row, column].state = newData.state;

        OnCellDataUpdated?.Invoke(row, column);
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
        return x >= 0 && x < _gridData.GetLength(0) &&
               z >= 0 && z < _gridData.GetLength(1);
    }
    #endregion
}
