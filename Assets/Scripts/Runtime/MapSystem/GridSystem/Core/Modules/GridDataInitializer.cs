using System.Collections.Generic;
using UnityEngine;

public class GridDataInitializer
{
    private readonly GridGenerator3D _generator;
    private readonly GridBlockerCollector _blockerCollector;

    public GridStateDataBase StateDataBase { get; private set; }
    public IReadOnlyList<Bounds> BlockingBounds { get; private set; }

    public GridDataInitializer(GridGenerator3D generator, GridBlockerCollector blockerCollector)
    {
        _generator = generator;
        _blockerCollector = blockerCollector;
    }

    public void Initialize()
    {
        StateDataBase = new GridStateDataBase(_generator.GridWidth, _generator.GridHeight);

        if (_blockerCollector != null)
        {
            _blockerCollector.Collect();
            BlockingBounds = _blockerCollector.BlockingBounds;
        }
        else
            Debug.LogError("_blockerCollector is null!!");
    }
}
