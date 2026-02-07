using UnityEngine;

public class TowerSnapper : MonoBehaviour
{
    [SerializeField] private bool snappingDuringUpdate = false;
    private GridManager _gridDataManager;

    private void Awake()
    {
        _gridDataManager = GridManager.Instance;
    }

    private void Update()
    {
        if (snappingDuringUpdate)
            SnapAllTowers();
    }

    private void SnapAllTowers()
    {

    }

    public void SnapTower()
    {

    }
}
