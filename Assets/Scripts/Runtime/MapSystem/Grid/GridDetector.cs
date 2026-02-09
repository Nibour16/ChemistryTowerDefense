using UnityEngine;

public struct GridBlockInfo
{
    public int x;
    public int z;
    public GameObject blocker;
}

public class GridDetector : BaseGridSecretary
{
    [SerializeField] private LayerMask blockingLayers;

    #region Detect if something is on the grid
    public bool IsCellBlocked(int x, int z)
    {
        Vector3 center = gridManager.GetCellCenter(x, z);

        return HasBlockingEntityAt(center);
    }

    public bool IsCellBlocked(Vector3 worldPos)
    {
        if (!gridManager.WorldToCell(worldPos, out int x, out int z))
            return true; // If over the grid will just return true anyway

        return IsCellBlocked(x, z);
    }

    private bool HasBlockingEntityAt(Vector3 cellCenter)
    {
        // must be half, preventing detecting other cells beside
        float half = gridManager.GridGenerator.CellSize * 0.45f;

        Collider[] hits = Physics.OverlapBox(
            cellCenter,
            new Vector3(half, 0.5f, half),
            Quaternion.identity,
            blockingLayers
        );

        return hits.Length > 0;
    }
    #endregion

    // Check the whole grid to return the used cells
    public void DetectAndApplyToGrid()
    {
        int w = gridManager.GridData.GetLength(0);
        int h = gridManager.GridData.GetLength(1);

        for (int x = 0; x < w; x++)
        {
            for (int z = 0; z < h; z++)
            {
                bool blocked = IsCellBlocked(x, z);
                var gridCellData = new GridCellData
                {
                    tower = null,
                    state = blocked ? GridCellState.NotPlaceable : GridCellState.Empty
                };

                gridManager.UpdateData(gridCellData, x, z);
            }
        }
    }
}
