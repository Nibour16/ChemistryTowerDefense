using UnityEngine;

public class GridData : MonoBehaviour
{
    private GridGenerator3D _grid;
    private BaseTower[,] _towers; // Grid data collector

    private void Awake()
    {
        _grid = GetComponent<GridGenerator3D>();
        InitializeTowerGrid();
    }

    private void InitializeTowerGrid()
    {
        int w = _grid.GridWidth;
        int h = _grid.GridHeight;
        _towers = new BaseTower[w, h];
    }
}
