using UnityEngine;

public interface IWorldPointerService : IMapService
{
    bool TryGetWorldPosition(Vector2 screenPos, out Vector3 worldPos);
    bool TryGetProjectedPosition(Vector2 screenPos, out Vector3 worldPos);
    bool TryRaycast(Vector2 screenPos, out RaycastHit hit);
}
