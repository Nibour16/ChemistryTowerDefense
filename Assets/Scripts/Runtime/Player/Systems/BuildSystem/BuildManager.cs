using System;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    private BuildDefinition _currentDefinition;
    public event Action<BuildDefinition> OnBuildRequested;

    protected override void Awake()
    {
        base.Awake();
    }
    
    public void OnTowerSelected(GameObject prefab)
    {
        if (prefab == null) return; // If prefab is empty, do nothing

        var definition = new BuildDefinition(prefab);   // Create new definition

        // If prefab does not contain tower stat, do not change to build state
        if (definition.TowerStat == null)   return;

        _currentDefinition = definition;    // Set the current definition

        // Request to enter build state
        OnBuildRequested?.Invoke(_currentDefinition);
    }

    public void ClearDefinition()
    {
        _currentDefinition = null;
    }
}
