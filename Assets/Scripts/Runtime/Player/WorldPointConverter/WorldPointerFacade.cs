using UnityEngine;

public class WorldPointerFacade
    : BaseFacade<IWorldPointerService, WorldPointerResolver>, IWorldPointerService
{
    public bool TryGetWorldPosition(Vector2 screenPos, out Vector3 worldPos)
        => Manager.TryGetWorldPosition(screenPos, out worldPos);

    public bool TryRaycast(Vector2 screenPos, out RaycastHit hit)
        => Manager.TryRaycast(screenPos, out hit);
}
