using System;
using UnityEngine;

public enum GridCellState
{
    Empty,
    NotPlaceable
}

public class GridCellData
{
    public BaseTower tower;
    public GridCellState state = GridCellState.Empty;
}

[RequireComponent(typeof(GridGenerator3D))]
[RequireComponent(typeof(GridDetector))]
public class GridManager : Singleton<GridManager>
{
    // Secretary class components of grid system
    private GridGenerator3D _gridGenerator; // Grid generator reference
    private GridDetector _gridDetector; // Grid detector reference
    
    // Grid system data collector
    private GridCellData[,] _gridData;

    // Subject during data update
    public event Action<int, int> OnCellDataUpdated;

    // Properties of secretary classes
    public GridGenerator3D GridGenerator => _gridGenerator;
    public GridDetector GridDetector => _gridDetector;
    public GridCellData[,] GridData => _gridData;

    #region Initialization
    protected override void Awake()
    {
        base.Awake();

        // Assign the secretary components that allows system members to apply
        _gridGenerator = GetComponent<GridGenerator3D>();
        _gridDetector = GetComponent<GridDetector>();

        // Assign all grid secretary components about their manager
        _gridDetector?.BindManager(this);
        _gridGenerator?.BindManager(this);

        // Initialize tower grid
        InitializeTowerGrid();
    }

    private void InitializeTowerGrid()
    {
        int w = _gridGenerator.GridWidth;
        int h = _gridGenerator.GridHeight;
        _gridData = new GridCellData[w, h];
    }
    #endregion

    #region Grid Translator Methods
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
    #endregion

    // Data Update Handler
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
}
