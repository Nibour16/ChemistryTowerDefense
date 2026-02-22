using UnityEngine;

public class BuildDefinition
{
    private GameObject _towerPrefab;
    private TowerStat _towerStat;

    public GameObject TowerPrefab => _towerPrefab;
    public TowerStat TowerStat => _towerStat;

    public BuildDefinition(GameObject prefab)
    {
        DefinePrefab(prefab);
        DefineStat();
    }

    private void DefinePrefab(GameObject prefab)
    {
        _towerPrefab = prefab;

        if (_towerPrefab == null)
        {
            Debug.LogError("TowerPrefab is not found.");
        }
    }

    private void DefineStat()
    {
        _towerStat = _towerPrefab.GetComponent<TowerStat>();

        if (_towerStat == null)
        {
            Debug.LogError("TowerPrefab must contain TowerStat component.");
        }
    }
}
