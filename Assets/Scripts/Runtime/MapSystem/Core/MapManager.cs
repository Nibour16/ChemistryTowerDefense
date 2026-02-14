using UnityEngine;

[RequireComponent(typeof(MapServiceRegistry))]
public class MapManager : Singleton<MapManager>
{
    // TODO: management of environment, grids, objects in the scene, loading system etc.
    private MapServiceRegistry _registry;

    protected override void Awake()
    {
        base.Awake();
        _registry = GetComponent<MapServiceRegistry>();

        _registry.RegisterAllServices();
    }

    public T GetService<T>() where T : class, IMapService
    {
        return _registry.GetService<T>();
    }
}
