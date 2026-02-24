using UnityEngine;

public class TowerPlacementHandler
{
    private readonly IGridService _grid;
    private readonly TowerSnapper _snapper;

    public TowerPlacementHandler(IGridService grid)
    {
        _grid = grid;
        _snapper = new TowerSnapper(grid);
    }

    public bool TryPromoteGhost(
        Vector3 worldPos, BuildDefinition definition, BuildPreviewHandler preview)
    {
        if (definition == null) return false;

        // Snap Tower
        if (!_snapper.TryGetSnappedPosition(worldPos, out var snappedPos, out int x, out int z))
            return false;

        // Check if placeable
        if (!_grid.IsPlaceable(x, z)) return false;

        // Get ghost from preview
        var ghost = preview.ExtractGhost();
        if (ghost == null) return false;

        // Fix the position
        ghost.transform.position = snappedPos;

        var tower = ghost.GetComponent<BaseTower>();
        if (tower == null) return false;

        // Occupy the grid cell
        if (!_grid.TryOccupy(x, z, tower)) return false;

        // Activate the tower
        ActivateTower(ghost);

        return true;
    }

    private void ActivateTower(GameObject tower)
    {
        foreach (var comp in tower.GetComponentsInChildren<MonoBehaviour>())
        {
            comp.enabled = true;
        }

        foreach (var col in tower.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }
    }
}
