using UnityEngine;

[RequireComponent(typeof(GridGenerator3D))]
public class GridVisualizer : MonoBehaviour
{
    [Header("Visual Setting")]
    [SerializeField] private bool displayCellMarkers = true;
    [SerializeField] private bool displayGridLine = true;

    [Header("Debug Visual Colour")]
    [SerializeField] private Color emptyColour = new Color(0, 1, 0, 0.3f);
    [SerializeField] private Color blockedColour = new Color(1, 0, 0, 0.3f);

    private GridManager _gridManager;
    private GridGenerator3D _gridGenerator;

    private void Awake()
    {
        _gridManager = GridManager.Instance;
        if (_gridManager == null)
        {
            Debug.LogError("Grid Manager is not found");
            return;
        }
        
        _gridGenerator = GetComponent<GridGenerator3D>();
    }

    private void Start()
    {
        CreateVisualGrid();
    }

    private void CreateVisualGrid() // Leading method
    {
        if (!TryCacheGridInfo(out float cell, out int w, out int h, out Vector3 corner, out float y))
            return;

        Transform root = CreateOrReplaceRoot();

        if(displayGridLine)
            DrawGridLines(root, corner, y, cell, w, h);

        var data = _gridManager.GridData;
        if (data == null) return;

        if (displayCellMarkers)
            DrawCellMarkers(root, data, y, cell, w, h);
    }

    #region Core Handler
    private bool TryCacheGridInfo(out float cell, out int w, out int h, out Vector3 corner, out float y)
    {
        cell = 0f;
        w = h = 0;
        corner = Vector3.zero;
        y = 0f;

        if (_gridGenerator == null)
        {
            Debug.LogError("GridGenerator3D not found on this GameObject.");
            return false;
        }

        cell = _gridGenerator.CellSize;
        w = _gridGenerator.GridWidth;
        h = _gridGenerator.GridHeight;

        // corner is the real bottom-left corner of the grid area (not the center of cell 0,0)
        corner = _gridGenerator.Origin - new Vector3(cell * 0.5f, 0f, cell * 0.5f);

        // lift a bit to avoid z-fighting with ground
        y = _gridGenerator.FixedY + 0.02f;

        return true;
    }

    private Transform CreateOrReplaceRoot()
    {
        // Remove old root if it exists (avoid duplicated drawing)
        Transform old = transform.Find("GridVisualRoot");
        if (old != null)
            Destroy(old.gameObject);

        Transform root = new GameObject("GridVisualRoot").transform;
        root.SetParent(transform, false);

        return root;
    }
    #endregion

    #region Line Drawer
    private void DrawGridLines(Transform root, Vector3 corner, float y, float cell, int w, int h)
    {
        // Lines parallel to Z
        for (int x = 0; x <= w; x++)
        {
            Vector3 a = new Vector3(corner.x + x * cell, y, corner.z);
            Vector3 b = new Vector3(corner.x + x * cell, y, corner.z + h * cell);
            CreateLine(root, $"Line_X_{x}", a, b, Color.black, 0.03f);
        }

        // Lines parallel to X
        for (int z = 0; z <= h; z++)
        {
            Vector3 a = new Vector3(corner.x, y, corner.z + z * cell);
            Vector3 b = new Vector3(corner.x + w * cell, y, corner.z + z * cell);
            CreateLine(root, $"Line_Z_{z}", a, b, Color.black, 0.03f);
        }
    }

    private void CreateLine(Transform parent, string name, Vector3 a, Vector3 b, Color color, float width)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);

        LineRenderer lr = go.AddComponent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.positionCount = 2;
        lr.SetPosition(0, a);
        lr.SetPosition(1, b);

        lr.startWidth = width;
        lr.endWidth = width;

        lr.material = new Material(Shader.Find("Unlit/Color"));
        lr.material.color = color;
    }
    #endregion

    #region Cell Marker Drawer
    private void DrawCellMarkers(Transform root, GridCellData[,] data, float y, float cell, int w, int h)
    {
        for (int x = 0; x < w; x++)
        {
            for (int z = 0; z < h; z++)
            {
                GridCellState state = data[x, z]?.state ?? GridCellState.Empty;
                Color c = state == GridCellState.NotPlaceable ? blockedColour : emptyColour;

                Vector3 center = _gridManager.GetCellCenter(x, z);
                center.y = y - 0.01f;

                GameObject marker = CreateCellMarker(root, x, z, center);
                SetMarkerWorldScale(marker.transform, cell);
                SetMarkerColor(marker, c);
            }
        }
    }

    private GameObject CreateCellMarker(Transform root, int x, int z, Vector3 worldPos)
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        marker.name = $"Cell_{x}_{z}";
        marker.transform.SetParent(root, false);
        marker.transform.position = worldPos;

        Destroy(marker.GetComponent<Collider>());
        return marker;
    }

    private void SetMarkerWorldScale(Transform marker, float cell)
    {
        // Thin tile: XZ cell, almost flat in Y
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
        r.material = new Material(Shader.Find("Unlit/Color"));
        r.material.color = c;
    }
    #endregion
}
