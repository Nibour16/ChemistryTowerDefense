using UnityEngine;

public abstract class PooledMonoBehaviour<T> : MonoBehaviour, IPoolable
    where T : PooledMonoBehaviour<T>
{
    protected bool isActive = false;
    private ObjectPool<T> _pool;

    public void SetPool(ObjectPool<T> pool)
    {
        _pool = pool;
    }

    protected void ReturnToPool()
    {
        if (!isActive) return;

        isActive = false;
        _pool?.Return((T)(object)this);
    }

    public virtual void OnSpawn()
    {
        isActive = true;
        gameObject.SetActive(true);
    }
    public virtual void OnDespawn()
    {
        isActive = false;
        gameObject.SetActive(false);
    }
}
