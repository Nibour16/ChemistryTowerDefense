using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IPoolable
{
    private readonly T _prefab;
    private readonly Transform _parent;
    private readonly Queue<T> _pool = new();

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            var obj = Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public T Get(Vector3 pos, Quaternion rot)
    {
        T obj = _pool.Count > 0 ? _pool.Dequeue()
                               : Object.Instantiate(_prefab, _parent);

        obj.transform.SetPositionAndRotation(pos, rot);
        obj.OnSpawn();
        return obj;
    }

    public void Return(T obj)
    {
        obj.OnDespawn();
        _pool.Enqueue(obj);
    }
}
