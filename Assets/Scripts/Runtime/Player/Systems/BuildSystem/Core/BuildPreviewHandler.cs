using UnityEngine;

public class BuildPreviewHandler : BaseBuildSecretary
{
    [SerializeField] private string buildAnchorName = "BuildAnchor";

    private GameObject _ghost;
    private Transform _anchor;
    private Vector3 _anchorOffset;

    public void CreateGhost()
    {
        if (manager.CurrentDefinition == null)
            return;

        var prefab = manager.CurrentDefinition.TowerPrefab;

        _ghost = Instantiate(prefab);
        _anchor = _ghost.transform.Find(buildAnchorName);

        if (_anchor != null)
            _anchorOffset = _ghost.transform.position - _anchor.position;
        else
        {
            Debug.LogWarning("Anchor is not found");
            _anchorOffset = default;
        }

        MakeGhostVisual(prefab);
    }

    private void MakeGhostVisual(GameObject ghost)
    {
        // Stop all momobehaviours
        foreach (var comp in ghost.GetComponentsInChildren<MonoBehaviour>())
        {
            comp.enabled = false;
        }

        // Stop all collisions
        foreach (var col in ghost.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
    }

    public GameObject ExtractGhost()
    {
        var result = _ghost;
        _ghost = null;
        return result;
    }

    public void DestroyGhost()
    {
        if (_ghost != null)
            Destroy(_ghost);
    }

    public void ApplyGhostVisualPosition(Vector3 worldPos, out Vector3 finalPos)
    {
        if (_ghost == null)
        {
            Debug.LogWarning("Ghost does not exist, return the final position as default value");
            finalPos = default;
            return;
        }

        finalPos = worldPos + _anchorOffset;
        _ghost.transform.position = finalPos;
    }

    public void ShowGhost()
    {
        if (_ghost != null && !_ghost.activeSelf)
            _ghost.SetActive(true);
    }

    public void HideGhost()
    {
        if (_ghost != null && _ghost.activeSelf)
            _ghost.SetActive(false);
    }
}