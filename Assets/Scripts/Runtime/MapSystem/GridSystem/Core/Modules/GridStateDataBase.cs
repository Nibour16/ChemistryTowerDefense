using UnityEngine;

public class GridStateDataBase
{
    private readonly GridCellData[,] _data;

    public int Width => _data.GetLength(0);
    public int Height => _data.GetLength(1);

    public GridStateDataBase(int width, int height)
    {
        _data = new GridCellData[width, height];
    }

    public GridCellData GetCell(int x, int z)
    {
        return _data[x, z];
    }

    public GridCellState GetState(int x, int z)
    {
        return _data[x, z]?.state ?? GridCellState.Empty;
    }

    public void SetState(int x, int z, GridCellState state)
    {
        if (_data[x, z] == null)
            _data[x, z] = new GridCellData();

        _data[x, z].state = state;
    }

    public bool IsAvailable(int x, int z)
    {
        return GetState(x, z) == GridCellState.Empty;
    }

    public bool IsInside(int x, int z)
    {
        if (_data == null)
            return false;

        return x >= 0 && x < _data.GetLength(0) &&
               z >= 0 && z < _data.GetLength(1);
    }
}
