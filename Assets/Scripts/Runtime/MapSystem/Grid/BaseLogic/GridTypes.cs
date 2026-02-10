using UnityEngine;

public enum GridCellState
{
    Empty,
    NotPlaceable
}

public class GridCellData
{
    public BaseTower tower;
    public GridCellState state = GridCellState.Empty;
}

public struct GridBlockInfo
{
    public int x;
    public int z;
    public GameObject blocker;
}