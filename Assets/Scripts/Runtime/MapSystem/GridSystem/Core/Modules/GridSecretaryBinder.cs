using UnityEngine;

public class GridSecretaryBinder
{
    private readonly GridManager _manager;

    public GridGenerator3D Generator { get; private set; }
    public GridDetector Detector { get; private set; }
    public GridBlockerCollector BlockerCollector { get; private set; }

    public GridSecretaryBinder(GridManager manager)
    {
        _manager = manager;
    }

    public void Bind()
    {
        Generator = _manager.GetComponent<GridGenerator3D>();
        Detector = _manager.GetComponent<GridDetector>();
        BlockerCollector = _manager.BlockerCollector;

        if (Generator != null)
            Generator.BindManager(_manager);
        else
            Debug.LogError("Generator does not exist in the object!");

        if (Detector != null)
            Detector.BindManager(_manager);
        else
            Debug.LogError("Detector does not exist in the object!");

        if (BlockerCollector != null)
            BlockerCollector.BindManager(_manager);
        else
            Debug.LogError("Blocker Collector is not assigned!");
    }
}
