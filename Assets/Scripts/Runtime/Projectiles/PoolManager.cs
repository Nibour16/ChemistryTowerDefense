using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private readonly Dictionary<Type, object> _pools = new();

    public void RegisterPool<T>(T prefab, int initialSize = 10)
       where T : MonoBehaviour, IPoolable
    {
        var type = typeof(T);

        if (_pools.ContainsKey(type))
            return;

        var pool = new ObjectPool<T>(prefab, initialSize, transform);
        _pools.Add(type, pool);
    }

    // Get Pool
    public T Get<T>(Vector3 pos, Quaternion rot)
        where T : MonoBehaviour, IPoolable
    {
        var type = typeof(T);

        if (!_pools.TryGetValue(type, out var poolObj))
            throw new Exception($"No pool registered for type {type}");

        var pool = (ObjectPool<T>)poolObj;
        return pool.Get(pos, rot);
    }

    // Return Pool
    public void Return<T>(T obj)
        where T : MonoBehaviour, IPoolable
    {
        var type = typeof(T);

        if (!_pools.TryGetValue(type, out var poolObj))
            throw new Exception($"No pool registered for type {type}");

        var pool = (ObjectPool<T>)poolObj;
        pool.Return(obj);
    }
}
