using UnityEngine;
public interface IGridService
{
    Vector3 GetCellCenter(int x, int z);
    bool WorldToCell(Vector3 worldPos, out int x, out int z);
    bool IsCellBlocked(int x, int z);
    bool IsInsideGrid(int x, int z);
}
