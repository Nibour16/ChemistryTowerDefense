using UnityEngine;

public class TowerSnapper
{
    private readonly IGridService _grid;

    public TowerSnapper(IGridService grid)
    {
        _grid = grid;
    }

    public bool TryGetSnappedPosition(
        Vector3 worldPos, out Vector3 snappedPos, out int cellX, out int cellZ)
    {
        if (!_grid.WorldToCell(worldPos, out cellX, out cellZ))
        {
            snappedPos = default;
            return false;
        }

        var center = _grid.GetCellCenter(cellX, cellZ);
        snappedPos = new Vector3(center.x, worldPos.y, center.z);   // Keep the y position

        return true;
    }
}
