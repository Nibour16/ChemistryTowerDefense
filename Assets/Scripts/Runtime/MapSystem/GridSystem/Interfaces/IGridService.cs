using UnityEngine;
public interface IGridService : IMapService
{
    Vector3 GetCellCenter(int x, int z);
    bool WorldToCell(Vector3 worldPos, out int x, out int z);
    bool IsInsideGrid(int x, int z);

    // Terrain
    bool IsCellBlocked(int x, int z);

    // State
    GridCellState GetState(int x, int z);
    bool IsPlaceable(int x, int z);
    bool TryOccupy(int x, int z, BaseTower tower);
    void ClearOccupation(int x, int z);
}
