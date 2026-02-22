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

        /*// Change the material colour
        foreach (var renderer in ghost.GetComponentsInChildren<Renderer>())
        {
            renderer.sharedMaterial.color = new Color(0, 1, 0, 0.5f);
        }*/
    }

    public void DestroyGhost()
    {
        if (_ghost != null)
            Destroy(_ghost);
    }

    public void UpdateGhostPosition(Vector3 worldPos)
    {
        if (_ghost == null)
            return;

        _ghost.transform.position = worldPos + _anchorOffset;
    }
}