using UnityEngine;

public class GridFacade : BaseGridSecretary, IGridService
{
    #region Main
    public Vector3 GetCellCenter(int x, int z)
        => gridManager.GetCellCenter(x, z);

    public bool WorldToCell(Vector3 worldPos, out int x, out int z)
        => gridManager.WorldToCell(worldPos, out x, out z);

    public bool IsInsideGrid(int x, int z)
        => gridManager.IsInsideGrid(x, z);
    #endregion

    #region Terrain
    public bool IsCellBlocked(int x, int z)
        => gridManager.IsCellBlocked(x, z);
    #endregion

    #region State
    public GridCellState GetState(int x, int z)
    => gridManager.StateDataBase.GetState(x, z);

    public bool IsPlaceable(int x, int z)
        => gridManager.StateDataBase.IsAvailable(x, z);

    public bool TryOccupy(int x, int z, BaseTower tower)
        => gridManager.TryOccupy(x, z, tower);

    public void ClearOccupation(int x, int z)
        => gridManager.ClearOccupation(x, z);
    #endregion
}
