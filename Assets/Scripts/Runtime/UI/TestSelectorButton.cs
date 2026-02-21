using UnityEngine;

public class TestSelectorButton : BaseUIButton
{
    [SerializeField] private GameObject towerObject;
    private PlayerManager _playerManager;

    protected override void Awake()
    {
        base.Awake();
        _playerManager = PlayerManager.Instance;
    }

    public override void OnHovered() { }

    public override void OnHoverExit() { }

    public override void OnSelected()
    {
        _playerManager.BuildManager.OnTowerSelected(towerObject);
    }
}
