using UnityEngine;

public class BuildPreviewHandler : BaseBuildSecretary
{
    private GameObject _ghost;

    public void CreateGhost()
    {
        if (manager.CurrentDefinition == null)
            return;

        var prefab = manager.CurrentDefinition.TowerPrefab;

        _ghost = Instantiate(prefab);

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

        // Change the material colour
        foreach (var renderer in ghost.GetComponentsInChildren<Renderer>())
        {
            renderer.material.color = new Color(0, 1, 0, 0.5f);
        }
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

        _ghost.transform.position = worldPos;
    }
}