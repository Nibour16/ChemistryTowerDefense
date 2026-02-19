using UnityEngine;

public class GridCoordinateSystem
{
    private readonly GridGenerator3D _generator;
    private readonly GridStateDataBase _db;

    public GridCoordinateSystem(GridGenerator3D generator, GridStateDataBase db)
    {
        _generator = generator;
        _db = db;
    }

    public Vector3 GetCellCenter(int x, int z)
    {
        return new Vector3(
            _generator.Origin.x + x * _generator.CellSize,
            _generator.FixedY,
            _generator.Origin.z + z * _generator.CellSize
        );
    }

    public bool WorldToCell(Vector3 worldPos, out int x, out int z)
    {
        Vector3 localPos = worldPos - _generator.Origin;

        x = Mathf.FloorToInt(localPos.x / _generator.CellSize);
        z = Mathf.FloorToInt(localPos.z / _generator.CellSize);

        return IsInsideGrid(x, z);
    }

    public bool IsInsideGrid(int x, int z)
    {
        return _db != null && _db.IsInside(x, z);
    }
}
