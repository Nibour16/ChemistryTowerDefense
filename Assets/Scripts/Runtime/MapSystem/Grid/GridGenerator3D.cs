using UnityEngine;

public class GridGenerator3D : MonoBehaviour
{
    [SerializeField] private float _cellSize = 1f; // Size of each grid cell
    [SerializeField] private int maxGridWidth = 30; // Max width of the grid
    [SerializeField] private int maxGridHeight = 30; // Max height of the grid

    private MeshRenderer _groundMesh; // Ground mesh reference (mesh renderer)

    private Vector3 _gridSize; // Size of the grid
    private Vector3 _origin; // Position of left-down corner of the ground, is the origin
    private int _gridWidth;
    private int _gridHeight;
    private float _fixedY; // Fix Grid's Y-axis

    #region Grid Properties
    public float CellSize => _cellSize;
    public Vector3 GridSize => _gridSize;
    public Vector3 Origin => _origin;
    public int GridWidth => _gridWidth;
    public int GridHeight => _gridHeight;
    public float FixedY => _fixedY;
    #endregion

    #region Initialization
    private void Awake()
    {
        // Get ground mesh from mesh renderer component, used to define the grid form
        _groundMesh = GetComponentInChildren<MeshRenderer>();

        if (_groundMesh == null)
        {
            Debug.LogError(
                "GridSystem requires a MeshRenderer on the same GameObject or one of its children."
            );
            return;
        }

        GenerateGrid();
    }

    private void GenerateGrid()
    {
        var bounds = _groundMesh.bounds;
        _gridSize = bounds.size;

        _gridWidth = Mathf.Min(Mathf.FloorToInt(_gridSize.x / _cellSize), maxGridWidth);
        _gridHeight = Mathf.Min(Mathf.FloorToInt(_gridSize.z / _cellSize), maxGridHeight);

        _fixedY = bounds.max.y; // The grid must be on the top of the ground

        float gridWorldSizeX = _gridWidth * _cellSize;
        float gridWorldSizeZ = _gridHeight * _cellSize;

        float padX = (_gridSize.x - gridWorldSizeX) * 0.5f;
        float padZ = (_gridSize.z - gridWorldSizeZ) * 0.5f;

        _origin = new Vector3(
            bounds.min.x + padX + _cellSize * 0.5f,
            _fixedY,
            bounds.min.z + padZ + _cellSize * 0.5f
        );
    }
    #endregion
}
