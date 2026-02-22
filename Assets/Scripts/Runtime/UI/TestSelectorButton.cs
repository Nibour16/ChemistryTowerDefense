using UnityEngine;

public class TestSelectorButton : BaseUIButton
{
    [SerializeField] private GameObject towerObject;
    private IBuildService _buildService;

    protected override void Awake()
    {
        base.Awake();
        _buildService = ServiceCollector.Build;
    }

    public override void OnHovered() { }

    public override void OnHoverExit() { }

    public override void OnSelected()
    {
        _buildService.OnTowerSelected(towerObject);
    }
}
