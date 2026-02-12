using UnityEngine;

public class TowerSnapper : BaseGridSystem
{
    [SerializeField] private bool snappingDuringUpdate = false;

    private void Update()
    {
        if (snappingDuringUpdate)
            SnapAllTowers();
    }

    private void SnapAllTowers()
    {
        GridCellData[,] grid = gridManager.GridData;

        // Data is empty, no need to snap
        if (grid == null)
            return;

        // Get width and height
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridCellData cell = grid[x, z];
                if (cell == null || cell.tower == null)
                    continue;

                SnapTower(cell.tower);
            }
        }
    }

    public void SnapTower(BaseTower tower)
    {
        if (tower == null)
            return;

        Vector3 worldPos = tower.transform.position;

        if (!gridManager.WorldToCell(worldPos, out int x, out int z))
            return;

        Vector3 snappedPos = gridManager.GetCellCenter(x, z);
        tower.transform.position = snappedPos;

        if (gridManager != null)
        {
            gridManager.ApplyStateToCell(x, z, true);
        }
    }

    /* We might change snapper to be like this:
    public void SnapTowerToCell(BaseTower tower, int x, int z)
    {
        if (tower == null)
            return;

        tower.transform.position = gridManager.GetCellCenter(x, z);
    }

    public void SnapAllTowers()
    {
        var grid = gridManager.GridData;
        if (grid == null)
            return;

        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var cell = grid[x, z];
                if (cell?.tower != null)
                    SnapTowerToCell(cell.tower, x, z);
            }
        }
    }*/
}
