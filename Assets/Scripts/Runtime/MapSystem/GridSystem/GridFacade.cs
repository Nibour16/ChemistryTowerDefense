using UnityEngine;

public class GridFacade : BaseFacade<IGridService, GridManager>, IGridService
{
    #region Main
    public Vector3 GetCellCenter(int x, int z)
        => Manager.GetCellCenter(x, z);

    public bool WorldToCell(Vector3 worldPos, out int x, out int z)
        => Manager.WorldToCell(worldPos, out x, out z);

    public bool IsInsideGrid(int x, int z)
        => Manager.IsInsideGrid(x, z);
    #endregion

    #region Terrain
    public bool IsCellBlocked(int x, int z)
        => Manager.IsCellBlocked(x, z);
    #endregion

    #region State
    public GridCellState GetState(int x, int z)
        => Manager.StateDataBase.GetState(x, z);

    public bool IsPlaceable(int x, int z)
        => Manager.StateDataBase.IsAvailable(x, z);

    public bool TryOccupy(int x, int z, BaseTower tower)
        => Manager.TryOccupy(x, z, tower);

    public void ClearOccupation(int x, int z)
        => Manager.ClearOccupation(x, z);
    #endregion
}
