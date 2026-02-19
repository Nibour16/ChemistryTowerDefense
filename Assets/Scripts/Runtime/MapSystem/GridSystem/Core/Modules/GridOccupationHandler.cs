using System;
using UnityEngine;

public class GridOccupationHandler
{
    private readonly GridStateDataBase _db;
    private readonly Action<int, int> _notify;

    public GridOccupationHandler(
        GridStateDataBase db,
        Action<int, int> notifyCallback)
    {
        _db = db;
        _notify = notifyCallback;
    }

    public bool TryOccupy(int x, int z, BaseTower tower)
    {
        if (_db == null)
            return false;

        if (!_db.IsInside(x, z))
            return false;

        if (_db.GetState(x, z) != GridCellState.Empty)
            return false;

        // Set to Occupied State
        _db.SetState(x, z, GridCellState.TowerOccupied);

        // Record Tower Reference
        var cell = _db.GetCell(x, z);
        if (cell != null)
            cell.tower = tower;

        _notify?.Invoke(x, z);

        return true;
    }

    public void ClearOccupation(int x, int z)
    {
        if (_db == null)
            return;

        if (!_db.IsInside(x, z))
            return;

        if (_db.GetState(x, z) != GridCellState.TowerOccupied)
            return;

        var cell = _db.GetCell(x, z);
        if (cell != null)
            cell.tower = null;

        _db.SetState(x, z, GridCellState.Empty);

        _notify?.Invoke(x, z);
    }
}
