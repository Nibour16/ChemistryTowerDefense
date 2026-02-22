using UnityEngine;

public class WorldPointerResolver : MonoBehaviour
{
    [SerializeField] private LayerMask raycastMask;
    [SerializeField, Min(10f)] private float raycastDistance = 100f;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public bool TryGetWorldPosition(Vector2 screenPos, out Vector3 worldPos)
    {
        if (TryRaycast(screenPos, out RaycastHit hit))
        {
            worldPos = hit.point;
            return true;
        }

        worldPos = default;
        return false;
    }

    public bool TryRaycast(Vector2 screenPos, out RaycastHit hit)
    {
        Ray ray = _camera.ScreenPointToRay(screenPos);
        return Physics.Raycast(ray, out hit, raycastDistance, raycastMask);
    }
}
