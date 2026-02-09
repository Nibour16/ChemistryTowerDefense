using System.Collections.Generic;
using UnityEngine;

public struct GridBlockInfo
{
    public int x;
    public int z;
    public GameObject blocker;
}

public class GridDetector : MonoBehaviour
{
    [SerializeField] private LayerMask blockingLayers;

    public bool IsCellBlocked(int x, int z)
    {
        return false;
    }
    public bool IsCellBlocked(Vector3 worldPos)
    {
        return false;
    }

    public BaseTower GetTowerOnCell(int x, int z)
    {
        return null;
    }
    public IEnumerable<GridBlockInfo> DetectAllOccupiedCells()
    {
        return null;
    }
}
