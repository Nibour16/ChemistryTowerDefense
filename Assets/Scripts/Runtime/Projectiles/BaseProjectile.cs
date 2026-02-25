using UnityEngine;

public abstract class PooledMonoBehaviour<T> : MonoBehaviour, IPoolable
    where T : PooledMonoBehaviour<T>
{
    private ObjectPool<T> _pool;

    public void SetPool(ObjectPool<T> pool)
    {
        _pool = pool;
    }

    protected void ReturnToPool()
    {
        _pool?.Return((T)(object)this);
    }

    public abstract void OnSpawn();
    public abstract void OnDespawn();
}
