public class GridSecretaryBinder
{
    private readonly GridManager _manager;

    public GridGenerator3D Generator { get; private set; }
    public GridDetector Detector { get; private set; }
    public GridBlockerCollector BlockerCollector { get; private set; }
    public GridFacade GridFacade { get; private set; }

    public GridSecretaryBinder(GridManager manager)
    {
        _manager = manager;
    }

    public void Bind()
    {
        Generator = _manager.GetComponent<GridGenerator3D>();
        Detector = _manager.GetComponent<GridDetector>();
        BlockerCollector = _manager.GetComponent<GridBlockerCollector>();
        GridFacade = _manager.GetComponent<GridFacade>();

        if (Generator != null)
            Generator.BindManager(_manager);

        if (Detector != null)
            Detector.BindManager(_manager);

        if (BlockerCollector != null)
            BlockerCollector.BindManager(_manager);

        if (GridFacade != null)
            GridFacade.BindManager(_manager);
    }
}
