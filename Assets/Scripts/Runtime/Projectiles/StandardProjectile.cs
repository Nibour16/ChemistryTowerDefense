using UnityEngine;

public class StandardProjectile : MonoBehaviour, IPoolable
{
    [SerializeField] protected float baseDamage = 20f;
    [SerializeField] protected float speed = 5f;
    
    private bool _isActive = false;
    public float BaseDamage => baseDamage;

    public void OnSpawn()
    {
        _isActive = true;
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        _isActive = false;
        gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if (!_isActive) return;

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!_isActive) return;

        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.OnTakenDamage(baseDamage);
            PoolManager.Instance.Return(this);
        }
    }
}
