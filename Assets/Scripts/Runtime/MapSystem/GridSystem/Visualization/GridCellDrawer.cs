using System.Collections.Generic;
using UnityEngine;

public class GridCellDrawer : MonoBehaviour
{
    [Header("Cell Colours")]
    [SerializeField] private List<GridCellStateColour> stateColours = new();

    [Header("Advanced Setting")]
    [SerializeField] private string cellNamePrefix = "Cell";

    private Dictionary<(int x, int z), GameObject> _cellMarkers = new();
    private Dictionary<GridCellState, Color> _colourMap;

    private Material _sharedMaterial;
    private MaterialPropertyBlock _mpb;

    #region Life Cycle
    private void Awake()
    {
        InitializeInternal();
    }

    private void OnDestroy()
    {
        if (_sharedMaterial != null)
            Destroy(_sharedMaterial);
    }

    private void InitializeInternal()
    {
        // Cache Material
        _mpb = new MaterialPropertyBlock();
        _sharedMaterial = new Material(Shader.Find("Unlit/Color"));

        // Setup colour dictionary data
        _colourMap = new Dictionary<GridCellState, Color>();

        // Assign colour dictionary data from state colours class list
        foreach (var entry in stateColours)
        {
            if (!_colourMap.ContainsKey(entry.state))
                _colourMap.Add(entry.state, entry.colour);
        }
    }
    #endregion

    #region Public API
    public void DrawAll(
        Transform root, System.Func<int, int, GridCellState> stateGetter,
        System.Func<int, int, Vector3> centerGetter,
        float y, float cell, int width, int height)
    {
        // Visualizing all cells in the grid
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                SetCellMarker(root, x, z, stateGetter, centerGetter, y, cell);
            }
        }
    }

    public void UpdateSingle(
        Transform root, int x, int z, System.Func<int, int, GridCellState> stateGetter,
        System.Func<int, int, Vector3> centerGetter, float y, float cell)
    {
        // Update Single Cell
        SetCellMarker(root, x, z, stateGetter, centerGetter, y, cell);
    }

    public void Clear()
    {
        // Clear all visualized cells in the grid
        foreach (var marker in _cellMarkers.Values)
        {
            if (marker != null)
                Destroy(marker);
        }

        _cellMarkers.Clear();   // Never forget to clear the cell marker data
    }
    #endregion

    #region Internal Drawing Logic
    /// <summary>
    /// Func<...> is a variable type called function
    /// Which allows you to return a specific method as a reference
    /// Always have a return value with a specific type on the last one
    /// 
    /// - Example Without Inputs: 
    /// Func<string> getMessage = () => "Hello, World!"; 
    /// // getMessage will give you the string type output: "Hello, World!"
    /// - Example With Inputs:
    /// Func<int, int, int> add = (x, y) => x + y;
    /// // add will give you the result of a integer type equal to: x + y
    /// 
    /// </summary>
    
    private void SetCellMarker(
        Transform root, int x, int z,
        System.Func<int, int, GridCellState> stateGetter,
        System.Func<int, int, Vector3> centerGetter,
        float y, float cell)
    {
        GridCellState state = stateGetter(x, z);
        Color c = GetMarkerColour(state);

        if (!_cellMarkers.TryGetValue((x, z), out GameObject marker))
            // If specific cell marker data has value
        {
            Vector3 center = centerGetter(x, z);
            center.y = y - 0.01f;
            // Prevent if the height are the same objects will fighting each other to make flashing visual

            marker = CreateCellMarker(root, x, z, center);
            SetMarkerWorldScale(marker.transform, cell);

            _cellMarkers[(x, z)] = marker;  // Update the marker data
        }

        SetMarkerColour(marker, c);  // Update the marker colour
    }

    private GameObject CreateCellMarker(Transform root, int x, int z, Vector3 worldPos)
    {
        /// <Summary>
        /// Creating marker as cubes by using CreatePrimitive() method 
        /// i.e. CreatePrimitive(PrimitiveType.Cube)
        /// 
        /// Similar as Instantiate() method
        /// But CreatePrimitive() is to create a new object, Instantiate() is to duplicate a prefab we have
        /// /<Summary>
        
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        marker.name = GetCellName(x, z);
        marker.transform.SetParent(root, false);
        marker.transform.position = worldPos;
        marker.layer = root.gameObject.layer;

        Destroy(marker.GetComponent<Collider>());
        return marker;
    }
    #endregion

    #region State Mapping
    private Color GetMarkerColour(GridCellState state)
    {
        if (_colourMap.TryGetValue(state, out var c))
            return c;   // Just to get the result from the colour data

        return Color.magenta; // If not setting up will give a magenta colour (usually magenta in Unity)
    }
    #endregion

    #region Marker Utilities
    private string GetCellName(int x, int z)
    {
        return $"{cellNamePrefix}_{x}_{z}"; // Cell name format will be like this
    }

    private void SetMarkerWorldScale(Transform marker, float cell)
    {
        /// <summary>
        /// We use the desired world scale to divide their parent world scale to get the desired result
        /// Because this can prevent their parent world scale is not default (!= new Vector3(1, 1, 1)) 
        /// Otherwise the desired result cell size will be very inaccurate
        /// </summary>
        
        Vector3 desiredWorldScale = new Vector3(cell * 0.98f, 0.01f, cell * 0.98f);
        Vector3 parentWorldScale = marker.parent.lossyScale;

        // Ensure to check if one of parentWorldScale vector values is 0 to prevent "Zero Division"
        // If it is 0, just using desiredWorld value
        marker.localScale = new Vector3(
            parentWorldScale.x != 0 ? desiredWorldScale.x / parentWorldScale.x : desiredWorldScale.x,
            parentWorldScale.y != 0 ? desiredWorldScale.y / parentWorldScale.y : desiredWorldScale.y,
            parentWorldScale.z != 0 ? desiredWorldScale.z / parentWorldScale.z : desiredWorldScale.z
        );
    }

    private void SetMarkerColour(GameObject marker, Color c)
    {
        /// <summary>
        /// Applies color using MaterialPropertyBlock instead of modifying material directly.
        /// 
        /// This allows per-instance color changes without creating new material instances,
        /// preventing unnecessary material duplication and preserving GPU batching.
        /// </summary>
        
        var r = marker.GetComponent<Renderer>();

        r.sharedMaterial = _sharedMaterial;

        r.GetPropertyBlock(_mpb);
        _mpb.SetColor("_Color", c);
        r.SetPropertyBlock(_mpb);
    }
    #endregion
}