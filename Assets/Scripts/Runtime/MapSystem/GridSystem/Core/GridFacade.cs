using UnityEngine;

public class GridFacade : BaseGridSystem, IGridService
{
    public Vector3 GetCellCenter(int x, int z)
        => gridManager.GetCellCenter(x, z);

    public bool WorldToCell(Vector3 worldPos, out int x, out int z)
        => gridManager.WorldToCell(worldPos, out x, out z);

    public bool IsCellBlocked(int x, int z)
        => gridManager.IsCellBlocked(x, z);

    public bool IsInsideGrid(int x, int z)
        => gridManager.IsInsideGrid(x, z);
}
