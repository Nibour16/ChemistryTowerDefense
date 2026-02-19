using System;
using UnityEngine;

public class GridStateSynchronizer
{
    private readonly GridDetector _detector;
    private readonly GridStateDataBase _db;
    private readonly Action<int, int> _notify;

    public GridStateSynchronizer(
        GridDetector detector, GridStateDataBase db, Action<int, int> notifyCallback)
    {
        _detector = detector;
        _db = db;
        _notify = notifyCallback;
    }

    public void ApplyInitialState()
    {
        if (_detector == null || _db == null)
            return;

        var blockedMap = _detector.DetectAllCells();

        int w = _db.Width;
        int h = _db.Height;

        for (int x = 0; x < w; x++)
        {
            for (int z = 0; z < h; z++)
            {
                ApplyStateToCell(x, z, blockedMap[x, z]);
            }
        }
    }

    private void ApplyStateToCell(int x, int z, bool blocked)
    {
        GridCellState oldState = _db.GetState(x, z);

        if (ShouldPreventOverride(oldState))
            return;

        GridCellState newState =
            blocked ? GridCellState.NotPlaceable : GridCellState.Empty;

        if (oldState == newState)
            return;

        _db.SetState(x, z, newState);
        _notify?.Invoke(x, z);
    }

    private bool ShouldPreventOverride(GridCellState oldState)
    {
        return oldState == GridCellState.TowerOccupied;
    }

    public bool IsCellBlocked(int x, int z)
    {
        return _detector != null && _detector.IsCellBlocked(x, z);
    }
}
