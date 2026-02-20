using System.Collections.Generic;
using UnityEngine;

public class GridCellDrawer : MonoBehaviour
{
    [Header("Cell Colours")]
    [SerializeField] private List<GridCellStateColour> stateColors = new();

    [Header("Advanced Setting")]
    [SerializeField] private string cellNamePrefix = "Cell";

    private Dictionary<(int x, int z), GameObject> _cellMarkers = new();
    private Dictionary<GridCellState, Color> _colorMap;
    private Material _sharedMaterial;

    private void Awake()
    {
        Initialization();
    }

    private void Initialization()
    {
        _sharedMaterial = new Material(Shader.Find("Unlit/Color"));
        _colorMap = new Dictionary<GridCellState, Color>();

        foreach (var entry in stateColors)
        {
            if (!_colorMap.ContainsKey(entry.state))
                _colorMap.Add(entry.state, entry.color);
        }
    }

    public void Clear()
    {
        foreach (var marker in _cellMarkers.Values)
        {
            if (marker != null)
                Destroy(marker);
        }

        _cellMarkers.Clear();
    }

    public void DrawAll(
        Transform root,
        System.Func<int, int, GridCellState> stateGetter,
        System.Func<int, int, Vector3> centerGetter,
        float y, float cell, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                SetCellMarker(root, x, z, stateGetter, centerGetter, y, cell);
            }
        }
    }

    public void UpdateSingle(
        Transform root, int x, int z,
        System.Func<int, int, GridCellState> stateGetter,
        System.Func<int, int, Vector3> centerGetter,
        float y, float cell)
    {
        SetCellMarker(root, x, z, stateGetter, centerGetter, y, cell);
    }

    private void SetCellMarker(
        Transform root, int x, int z,
        System.Func<int, int, GridCellState> stateGetter,
        System.Func<int, int, Vector3> centerGetter,
        float y, float cell)
    {
        GridCellState state = stateGetter(x, z);
        Color c = GetMarkerColour(state);

        if (!_cellMarkers.TryGetValue((x, z), out GameObject marker))
        {
            Vector3 center = centerGetter(x, z);
            center.y = y - 0.01f;

            marker = CreateCellMarker(root, x, z, center);
            SetMarkerWorldScale(marker.transform, cell);

            _cellMarkers[(x, z)] = marker;
        }

        SetMarkerColor(marker, c);
    }

    private Color GetMarkerColour(GridCellState state)
    {
        if (_colorMap.TryGetValue(state, out var c))
            return c;

        return Color.magenta; // If not setting up will give a magenta colour (usually magenta in Unity)
    }

    private GameObject CreateCellMarker(Transform root, int x, int z, Vector3 worldPos)
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        marker.name = GetCellName(x, z);
        marker.transform.SetParent(root, false);
        marker.transform.position = worldPos;
        marker.layer = root.gameObject.layer;

        Destroy(marker.GetComponent<Collider>());
        return marker;
    }

    private string GetCellName(int x, int z)
    {
        return $"{cellNamePrefix}_{x}_{z}";
    }

    private void SetMarkerWorldScale(Transform marker, float cell)
    {
        Vector3 desiredWorld = new Vector3(cell * 0.98f, 0.01f, cell * 0.98f);
        Vector3 parentWorld = marker.parent.lossyScale;

        marker.localScale = new Vector3(
            parentWorld.x != 0 ? desiredWorld.x / parentWorld.x : desiredWorld.x,
            parentWorld.y != 0 ? desiredWorld.y / parentWorld.y : desiredWorld.y,
            parentWorld.z != 0 ? desiredWorld.z / parentWorld.z : desiredWorld.z
        );
    }

    private void SetMarkerColor(GameObject marker, Color c)
    {
        var r = marker.GetComponent<Renderer>();
        r.material = _sharedMaterial;
        r.material.color = c;
    }
}
