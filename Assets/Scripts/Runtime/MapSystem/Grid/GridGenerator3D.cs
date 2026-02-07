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

    public int GridWidth => _gridWidth;
    public int GridHeight => _gridHeight;

    private float _fixedY; // Fix Grid's Y-axis

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
        _gridSize = _groundMesh.bounds.size;

        _gridWidth = Mathf.Min(Mathf.FloorToInt(_gridSize.x / _cellSize), maxGridWidth);
        _gridHeight = Mathf.Min(Mathf.FloorToInt(_gridSize.z / _cellSize), maxGridHeight);

        // Starting from left-down corner point
        _origin = transform.position - _gridSize / 2f + new Vector3(_cellSize / 2f, 0, _cellSize / 2f);

        // Lock Y position
        _fixedY = _groundMesh.bounds.max.y; // The grid must be on the top of the ground
    }
    #endregion

    // Get world position of the center of the grid cell
    public Vector3 GetCellCenter(int x, int z)
    {
        return new Vector3(
            _origin.x + x * _cellSize,
            _fixedY,
            _origin.z + z * _cellSize
        );
    }

    // Convert world position to grid cell position
    public bool WorldToCell(Vector3 worldPos, out int x, out int z)
    {
        Vector3 localPos = worldPos - _origin;
        x = Mathf.FloorToInt(localPos.x / _cellSize);
        z = Mathf.FloorToInt(localPos.z / _cellSize);
        return x >= 0 && x < _gridWidth && z >= 0 && z < _gridHeight;
    }
}
