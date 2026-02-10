using UnityEngine;

public class GridDetector : BaseGridSecretary
{
    [SerializeField] private LayerMask blockingLayers;

    #region Detect if something is on the grid
    public bool IsCellBlocked(int x, int z)
    {
        return HasBlockingEntityAt(x, z);
    }

    public bool IsCellBlocked(Vector3 worldPos)
    {
        if (!gridManager.WorldToCell(worldPos, out int x, out int z))
            return true; // If over the grid will just return true anyway

        return IsCellBlocked(x, z);
    }

    private bool HasBlockingEntityAt(int x, int z)
    {
        // No manager or no blocking data will be result as "not blocked"
        if (gridManager == null || gridManager.BlockingBounds == null)
            return false;

        // Calculate the cell bounds in the world
        float cell = gridManager.GridGenerator.CellSize;
        Vector3 center = gridManager.GetCellCenter(x, z);

        Bounds cellBounds = new Bounds(
            new Vector3(center.x, center.y, center.z),
            new Vector3(cell, cell, cell)
        );

        // Rule-based intersection check to give the result
        var blockers = gridManager.BlockingBounds;

        foreach (var blocker in blockers)
        {
            if (cellBounds.Intersects(blocker))
                return true;
        }

        return false;
    }
    #endregion

    // TODO: Should be internal when all systems are stabilized
    // Detect all cells in the grid
    public bool[,] DetectAllCells()
    {
        int w = gridManager.GridData.GetLength(0);
        int h = gridManager.GridData.GetLength(1);

        bool[,] blockedMap = new bool[w, h];

        for (int x = 0; x < w; x++)
        {
            for (int z = 0; z < h; z++)
            {
                blockedMap[x, z] = IsCellBlocked(x, z);
            }
        }

        return blockedMap;
    }
}
