using UnityEngine;

public class StandardProjectile : PooledMonoBehaviour<StandardProjectile>
{
    [SerializeField] protected float baseDamage = 20f;
    [SerializeField] protected float speed = 5f;

    public float BaseDamage => baseDamage;

    private EnemyCharacter _target;

    public void Initialize(EnemyCharacter target)
    {
        _target = target;
    }

    protected virtual void Update()
    {
        if (_target == null)
            ReturnToPool();

        transform.position += 10f * Time.deltaTime * transform.forward;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.OnTakenDamage(baseDamage);
            ReturnToPool();
        }
    }

    public override void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public override void OnDespawn()
    {
        gameObject.SetActive(false);
    }
}
