using UnityEngine;

public class TowerSnapper : BaseGridSystem
{
    public bool TryGetSnappedPosition(
        Vector3 worldPos, out Vector3 snappedPos, out int cellX, out int cellZ)
    {
        if (!gridManager.WorldToCell(worldPos, out cellX, out cellZ))
        {
            snappedPos = default;
            return false;
        }
        else
        {
            snappedPos = gridManager.GetCellCenter(cellX, cellZ);
            return true;
        }
    }
}
