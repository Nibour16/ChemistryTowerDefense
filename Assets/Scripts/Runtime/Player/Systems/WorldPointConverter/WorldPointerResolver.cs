using UnityEngine;

public class WorldPointerResolver : MonoBehaviour
{
    [SerializeField] private float groundPlaneHeight = 0f;
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

    public bool TryGetProjectedPosition(Vector2 screenPos, out Vector3 worldPos)
    {
        Ray ray = GetPointRay(screenPos);

        Vector3 groundPlanePosition = new (0f, groundPlaneHeight, 0f);
        Plane groundPlane = new(Vector3.up, groundPlanePosition);

        if (groundPlane.Raycast(ray, out float enter))
        {
            worldPos = ray.GetPoint(enter);
            return true;
        }

        worldPos = default;
        return false;
    }

    public bool TryRaycast(Vector2 screenPos, out RaycastHit hit)
    {
        Ray ray = GetPointRay(screenPos);
        return Physics.Raycast(ray, out hit, raycastDistance, raycastMask);
    }

    private Ray GetPointRay(Vector2 screenPos)
        => _camera.ScreenPointToRay(screenPos);
}
