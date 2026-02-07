using UnityEngine;

public class TowerSnapper : MonoBehaviour
{
    [SerializeField] private bool snappingDuringUpdate = false;

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
