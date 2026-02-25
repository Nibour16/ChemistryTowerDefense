using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private readonly Dictionary<MonoBehaviour, object> _pools = new();

    public void RegisterPool<T>(T prefab, int initialSize = 10)
       where T : PooledMonoBehaviour<T>
    {
        if (_pools.ContainsKey(prefab)) return;

        var pool = new ObjectPool<T>(prefab, initialSize, transform);
        _pools.Add(prefab, pool);
    }

    public T Get<T>(T prefab, Vector3 pos, Quaternion rot)
        where T : PooledMonoBehaviour<T>
    {
        if (!_pools.TryGetValue(prefab, out var poolObj))
        {
            RegisterPool(prefab);
            poolObj = _pools[prefab];
        }

        var pool = (ObjectPool<T>)poolObj;
        return pool.Get(pos, rot);
    }

    public void Return<T>(T prefab, T obj) where T : PooledMonoBehaviour<T>
    {
        if (!_pools.TryGetValue(prefab, out var poolObj)) return;

        var pool = (ObjectPool<T>)poolObj;
        pool.Return(obj);
    }
}
