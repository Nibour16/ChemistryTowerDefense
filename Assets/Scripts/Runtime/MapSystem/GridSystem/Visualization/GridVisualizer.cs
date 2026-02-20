using UnityEngine;

[RequireComponent(typeof(GridLineDrawer))]
[RequireComponent(typeof(GridCellDrawer))]
public class GridVisualizer : BaseGridSystem
{
    private Transform _root;

    // Drawer References
    private GridLineDrawer _lineDrawer;
    private GridCellDrawer _cellDrawer;

    // Cached Values
    private float _cachedCellSize;
    private float _cachedY;
    private int _cachedWidth;
    private int _cachedHeight;

    #region Lifecycle
    protected override void Awake()
    {
        base.Awake();

        _lineDrawer = GetComponent<GridLineDrawer>();
        _cellDrawer = GetComponent<GridCellDrawer>();
    }

    private void OnEnable()
    {
        ClearVisualGrid();
        CreateVisualGrid();

        if (gridManager != null && _cellDrawer.enabled)
            gridManager.OnCellDataUpdated += UpdateCellMarker;
    }

    private void OnDisable()
    {
        if (gridManager != null)
            gridManager.OnCellDataUpdated -= UpdateCellMarker;

        ClearVisualGrid();
    }
    #endregion

    #region Grid Creation
    private void CreateVisualGrid()
    {
        if (!TryCacheGridInfo(
                out float cell, out int width, out int height, 
                out Vector3 corner, out float y))
            return;

        _root = CreateOrReplaceRoot();

        _cachedCellSize = cell;
        _cachedWidth = width;
        _cachedHeight = height;
        _cachedY = y;

        if (_lineDrawer != null && _lineDrawer.enabled)
        {
            _lineDrawer.DrawLines(
                _root, corner,
                _cachedY, _cachedCellSize,
                _cachedWidth, _cachedHeight);
        }

        if (_cellDrawer != null && _cellDrawer.enabled)
        {
            _cellDrawer.DrawAll(
                _root, gridManager.StateDataBase.GetState,
                gridManager.GetCellCenter, _cachedY,
                _cachedCellSize, _cachedWidth, _cachedHeight);
        }
    }

    private void ClearVisualGrid()
    {
        if (_root != null)
        {
            Destroy(_root.gameObject);
            _root = null;
        }

        if (_cellDrawer != null)
            _cellDrawer.Clear();
    }
    #endregion

    #region Event Update
    private void UpdateCellMarker(int x, int z)
    {
        if (_root == null)
            return;

        if (x < 0 || x >= _cachedWidth || z < 0 || z >= _cachedHeight)
            return;

        if (_cellDrawer != null)
        {
            _cellDrawer.UpdateSingle(
                _root,
                x,
                z,
                gridManager.StateDataBase.GetState,
                gridManager.GetCellCenter,
                _cachedY,
                _cachedCellSize);
        }
    }
    #endregion

    #region Helpers
    private bool TryCacheGridInfo(
        out float cell, out int width, out int height, 
        out Vector3 corner, out float y)
    {
        cell = 0f;
        width = height = 0;
        corner = Vector3.zero;
        y = 0f;

        if (gridGenerator == null)
        {
            Debug.LogError("GridGenerator3D not found.");
            return false;
        }

        cell = gridGenerator.CellSize;
        width = gridGenerator.GridWidth;
        height = gridGenerator.GridHeight;

        corner = gridGenerator.Origin -
                 new Vector3(cell * 0.5f, 0f, cell * 0.5f);

        y = gridGenerator.FixedY + 0.02f;

        return true;
    }

    private Transform CreateOrReplaceRoot()
    {
        Transform old = transform.Find("GridVisualRoot");
        if (old != null)
            Destroy(old.gameObject);

        Transform root = new GameObject("GridVisualRoot").transform;
        root.SetParent(transform, false);
        root.gameObject.layer = gameObject.layer;

        return root;
    }
    #endregion
}
