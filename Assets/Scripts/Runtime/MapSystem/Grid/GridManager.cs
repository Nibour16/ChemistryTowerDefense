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
public class GridManager : Singleton<GridManager>
{
    private GridGenerator3D _grid; // Grid reference
    private GridCellData[,] _gridData; // Grid data collector

    public GridCellData[,] GridData => _gridData;

    protected override void Awake()
    {
        base.Awake();
        _grid = GetComponent<GridGenerator3D>();
        InitializeTowerGrid();
    }

    private void InitializeTowerGrid()
    {
        int w = _grid.GridWidth;
        int h = _grid.GridHeight;
        _gridData = new GridCellData[w, h];
    }

    // Get world position of the center of the grid cell
    public Vector3 GetCellCenter(int x, int z)
    {
        return new Vector3(
            _grid.Origin.x + x * _grid.CellSize,
            _grid.FixedY,
            _grid.Origin.z + z * _grid.CellSize
        );
    }

    // Convert world position to grid cell position
    public bool WorldToCell(Vector3 worldPos, out int x, out int z)
    {
        Vector3 localPos = worldPos - _grid.Origin;
        x = Mathf.FloorToInt(localPos.x / _grid.CellSize);
        z = Mathf.FloorToInt(localPos.z / _grid.CellSize);
        return x >= 0 && x < _grid.GridWidth && z >= 0 && z < _grid.GridHeight;
    }

    public void UpdateData(GridCellData newData, int row, int column)
    {
        if (_gridData == null)
            return;

        if (row < 0 || row >= _grid.GridWidth ||
            column < 0 || column >= _grid.GridHeight)
            return;

        if (_gridData[row, column] == null)
            _gridData[row, column] = new GridCellData();

        _gridData[row, column].tower = newData.tower;
        _gridData[row, column].state = newData.state;
    }
}
